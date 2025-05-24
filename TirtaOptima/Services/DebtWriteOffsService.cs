using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class DebtWriteOffsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<DebtSummaryViewModel> GetDebtSummaries(DebtWriteOffsViewModel model)
        {
            var bulan = model.BulanSelect;
            var tahun = model.TahunSelect;

            return _context.DebtsManagements
                .Include(dm => dm.Pembayaran)
                .Include(dm => dm.Piutang!)
                    .ThenInclude(p => p.IdPelangganNavigation)
                        .ThenInclude(pn => pn.JenisNavigation)
                .Include(dm => dm.Piutang!)
                    .ThenInclude(p => p.IdPelangganNavigation)
                        .ThenInclude(pn => pn.KelurahanNavigation!)
                            .ThenInclude(kel => kel.KodeKecNavigation)
                .Where(dm => dm.DeletedAt == null)
                .Where(dm =>
                    (dm.Piutang != null && dm.Piutang.Bulan == bulan && dm.Piutang.Tahun == tahun) ||
                    (dm.Pembayaran != null && dm.Pembayaran.Bulan == bulan && dm.Pembayaran.Tahun == tahun)
                )
                .Where(dm =>
                    dm.Piutang != null &&
                    dm.Piutang.IdPelangganNavigation != null &&
                    dm.Piutang.IdPelangganNavigation.KelurahanNavigation != null &&
                    dm.Piutang.IdPelangganNavigation.KelurahanNavigation.KodeKecNavigation != null
                )
                .GroupBy(dm => new
                {
                    PiutangId = dm.PiutangId,
                    IdPelanggan = dm.Piutang!.IdPelanggan,
                    Nama = dm.Piutang.IdPelangganNavigation.Nama,
                    Nomor = dm.Piutang.IdPelangganNavigation.Nomor,
                    Jenis = dm.Piutang.IdPelangganNavigation.JenisNavigation!.Deskripsi,
                    Alamat = dm.Piutang.IdPelangganNavigation.Alamat,
                    Kelurahan = dm.Piutang.IdPelangganNavigation.KelurahanNavigation!.Nama,
                    Kecamatan = dm.Piutang.IdPelangganNavigation.KelurahanNavigation.KodeKecNavigation!.Nama
                })
                .Select(g => new DebtSummaryViewModel
                {
                    IdPiutang = g.Key.PiutangId,
                    IdPelanggan = g.Key.IdPelanggan,
                    NamaPelanggan = g.Key.Nama,
                    NomorPelanggan = g.Key.Nomor,
                    JenisPelanggan = g.Key.Jenis,
                    Alamat = g.Key.Alamat,
                    Kelurahan = g.Key.Kelurahan,
                    Kecamatan = g.Key.Kecamatan,
                    Periode = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(bulan)} | {tahun}",
                    TotalNominal = g.Sum(dm =>
                        dm.Status == "Kredit" ? -dm.Nominal : dm.Nominal
                    )
                })
                .ToList();
        }
        public DebtsManagement GetDebt(DebtWriteOffsViewModel model)
        {
            var bulan = model.BulanSelect;
            var tahun = model.TahunSelect;

            return _context.DebtsManagements
                .Include(dm => dm.Pembayaran)
                .Include(dm => dm.Piutang!)
                    .ThenInclude(p => p.IdPelangganNavigation)
                        .ThenInclude(pn => pn.JenisNavigation)
                .Include(dm => dm.Piutang!)
                    .ThenInclude(p => p.IdPelangganNavigation)
                        .ThenInclude(s => s.StatusNavigation)
                .Include(dm => dm.Piutang!)
                    .ThenInclude(p => p.IdPelangganNavigation)
                        .ThenInclude(pn => pn.KelurahanNavigation!)
                            .ThenInclude(kel => kel.KodeKecNavigation)
                .Where(dm => dm.DeletedAt == null)
                .Where(dm =>
                    (dm.Piutang != null &&
                     dm.Piutang.Bulan == bulan &&
                     dm.Piutang.Tahun == tahun &&
                     dm.Piutang.IdPelanggan == model.IdPelanggan)
                    ||
                    (dm.Pembayaran != null &&
                     dm.Pembayaran.Bulan == bulan &&
                     dm.Pembayaran.Tahun == tahun &&
                     dm.Pembayaran.IdPelanggan == model.IdPelanggan)
                )
                .Where(dm =>
                    dm.Piutang != null &&
                    dm.Piutang.IdPelangganNavigation != null &&
                    dm.Piutang.IdPelangganNavigation.KelurahanNavigation != null &&
                    dm.Piutang.IdPelangganNavigation.KelurahanNavigation.KodeKecNavigation != null
                ).First();
        }
        public void Delete(DebtWriteOffsViewModel input, long userid)
        {
            DebtsManagement debtsManagement = new DebtsManagement
            {
                Id = _context.DebtsManagements.Select(x => x.Id).Max() + 1,
                PiutangId = input.IdPiutang,
                Tanggal = DateTime.Now,
                Nominal = Math.Abs(input.Nominal),
                Status = "Dihapus",
                CreatedAt = DateTime.Now,
                CreatedBy = userid,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userid,
            };
            _context.DebtsManagements.Add(debtsManagement);
            _context.SaveChanges();
            ViewDebtsService service = new(_context);
            service.UpdateDebts();
        }
    }
}
