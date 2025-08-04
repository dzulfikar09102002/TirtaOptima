using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
	public class StrategyResultService(DatabaseContext context)
	{
		private readonly DatabaseContext _context = context;
		public List<Debt> GetDebts(StrategyResultViewModel input)
		{
			ViewDebtsHelper.UpdateDebts(_context);

			var debts = _context.Debts
				.Include(x => x.StrategyResults)
					.ThenInclude(x => x.Strategi)
				.Include(x => x.Pelanggan)
					.ThenInclude(p => p.JenisNavigation)
				.Include(x => x.Pelanggan)
					.ThenInclude(p => p.KelurahanNavigation!)
						.ThenInclude(k => k.KodeKecNavigation)
				.Where(x =>
					x.DeletedAt == null &&
					x.TanggalTerakhir.HasValue &&
					x.TanggalTerakhir.Value.Month == input.BulanSelect &&
					x.TanggalTerakhir.Value.Year == input.TahunSelect &&
					x.Nominal > 0)
				.ToList();

			return debts;
		}

		public List<ResultColumn> GetScore(StrategyResultViewModel input, long userid)
		{
			var debts = GetDebts(input)
			.Where(x =>
				!x.Collections.Any() ||
				x.StrategyResults.Any(r => r.Status == "pending"
				|| !x.StrategyResults.Any())
			);
			if (!debts.Any()) return new();

			// Hitung umur piutang
			var umurSemua = debts
				.Where(d => d.TanggalTerakhir.HasValue)
				.Select(d => (DateTime.Now - d.TanggalTerakhir!.Value).Days)
				.ToList();

			// Min/Max value
			decimal minNominal = debts.Min(d => d.Nominal);
			decimal maxNominal = debts.Max(d => d.Nominal);
			int minUmur = umurSemua.Min();
			int maxUmur = umurSemua.Max();

			// Bobot kriteria
			var criterias = _context.Criterias.ToDictionary(c => c.Name, c => c.Bobot);
			var lastIdResult = _context.StrategyResults.Any()
			? _context.StrategyResults.Max(x => x.Id)
			: 1;

			var results = new List<ResultColumn>();

			foreach (var d in debts)
			{
				int umurPiutang = (DateTime.Now - d.TanggalTerakhir!.Value).Days;

				decimal normNominal = Normalize(d.Nominal, minNominal, maxNominal);
				decimal normUmur = Normalize(umurPiutang, minUmur, maxUmur);
				decimal normJenis = JenisMapping.TryGetValue(d.Pelanggan.Jenis ?? "", out var jVal) ? jVal : 0m;
				decimal normStatus = StatusMapping.TryGetValue(d.Pelanggan.Status, out var sVal) ? sVal : 0m;
				decimal normLayanan = Normalize((decimal)(d.Pelanggan.KelurahanNavigation!.Layanan ?? 0m), 1, 5);
				decimal normAlamat = Normalize((decimal)(d.Pelanggan.KelurahanNavigation!.Jarak ?? 0m), 5, 1);

				decimal? score = 0;
				score += criterias.GetValueOrDefault("Nominal", 0) * normNominal;
				score += criterias.GetValueOrDefault("Umur Piutang", 0) * normUmur;
				score += criterias.GetValueOrDefault("Jenis Pelanggan", 0) * normJenis;
				score += criterias.GetValueOrDefault("Status Pelanggan", 0) * normStatus;
				score += criterias.GetValueOrDefault("Kualitas Layanan", 0) * normLayanan;
				score += criterias.GetValueOrDefault("Alamat", 0) * normAlamat;

				decimal scoreNominal = criterias.GetValueOrDefault("Nominal", 0) * normNominal ?? 0;
				decimal scoreUmur = criterias.GetValueOrDefault("Umur Piutang", 0) * normUmur ?? 0;
				decimal scoreJenis = criterias.GetValueOrDefault("Jenis Pelanggan", 0) * normJenis ?? 0;
				decimal scoreStatus = criterias.GetValueOrDefault("Status Pelanggan", 0) * normStatus ?? 0;
				decimal scoreLayanan = criterias.GetValueOrDefault("Kualitas Layanan", 0) * normLayanan ?? 0;
				decimal scoreAlamat = criterias.GetValueOrDefault("Alamat", 0) * normAlamat ?? 0;


				var policy = _context.Policies
					.Where(p => umurPiutang >= (int)(p.RentangWaktu ?? 0))
					.OrderByDescending(p => p.RentangWaktu)
					.FirstOrDefault();

				results.Add(new ResultColumn
				{
					Id = d.PelangganId,
					Pelanggan = $"<b>{d.Pelanggan.Id}</b> - {d.Pelanggan.Nama}",
					Nominal = scoreNominal,
					Umur = scoreUmur,
					Jenis = scoreJenis,
					Status = scoreJenis,
					Layanan = scoreLayanan,
					Alamat = scoreAlamat,
					Score = score ?? 0,
					Tindakan = policy?.NamaStrategi,
					PiutangId = d.Id
				});
				var existing = d.StrategyResults.FirstOrDefault();
				var roundedScore = Math.Round(score ?? 0, 4);
				if (existing == null)
				{
					_context.StrategyResults.Add(new StrategyResult
					{
						Id = lastIdResult,
						PiutangId = d.Id,
						StrategiId = policy!.Id,
						Score = roundedScore,
						Status = "pending",
						CreatedAt = DateTime.Now,
						CreatedBy = userid,
						UpdatedAt = DateTime.Now,
						UpdatedBy = userid
					});
					lastIdResult++;
				}
				else if (existing.Score != roundedScore || existing.StrategiId != policy!.Id)
				{
					existing.Score = roundedScore;
					existing.StrategiId = policy!.Id;
					existing.UpdatedAt = DateTime.Now;
					existing.UpdatedBy = userid;
				}

			}
			_context.SaveChanges();
			var patokan = results.OrderByDescending(x => x.Score).FirstOrDefault();
			if (patokan != null)
			{
				var patokanDebt = debts.FirstOrDefault(x => x.PelangganId == patokan.Id);
				var patokanKelurahan = patokanDebt?.Pelanggan.Kelurahan;
				var patokanKecamatan = patokanDebt?.Pelanggan.Kecamatan;

				// 2. Hitung ZonaGroup untuk setiap hasil
				foreach (var r in results)
				{
					var d = debts.FirstOrDefault(x => x.PelangganId == r.Id);
					if (d == null || d.Pelanggan == null)
					{
						r.ZonaGroup = 3; // default di luar kecamatan
						continue;
					}

					if (d.Pelanggan.Kelurahan == patokanKelurahan)
						r.ZonaGroup = 1; // Kelurahan sama
					else if (d.Pelanggan.Kecamatan == patokanKecamatan)
						r.ZonaGroup = 2; // Kecamatan sama
					else
						r.ZonaGroup = 3; // Luar kecamatan
				}

				// 3. Urutkan ulang berdasarkan zona & skor
				results = results
					.OrderBy(r => r.ZonaGroup)
					.ThenByDescending(r => r.Score)
					.ToList();
			}
			return results;
		}


		private decimal Normalize(decimal value, decimal min, decimal max)
		{
			if (max == min) return 1;
			return (value - min) / (max - min);
		}

		private decimal Normalize(int value, int min, int max)
		{
			if (max == min) return 1;
			return (decimal)(value - min) / (max - min);
		}


		private static readonly Dictionary<string, decimal> JenisMapping = new()
		{
			["1A"] = 0.05m,
			["1B"] = 0.10m,
			["1C"] = 0.15m,
			["1D"] = 0.20m,
			["1E"] = 0.25m,
			["2A"] = 0.30m,
			["3A"] = 0.35m,
			["3B"] = 0.40m,
			["3C"] = 0.45m,
			["3D"] = 0.50m,
			["3E"] = 0.55m,
			["3F"] = 0.60m,
			["3G"] = 0.65m,
			["3H"] = 0.70m,
			["3I"] = 0.75m,
			["3J"] = 0.80m,
			["3K"] = 0.85m,
			["4A"] = 0.90m
		};

		private static readonly Dictionary<long, decimal> StatusMapping = new()
		{
			[1] = 0.25m,
			[2] = 0.5m,
			[5] = 0.75m,
			[7] = 1.0m
		};

	}
}
