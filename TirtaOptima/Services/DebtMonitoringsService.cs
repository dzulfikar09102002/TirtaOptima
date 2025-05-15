using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class DebtMonitoringsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Policy> GetPolicies() => [.._context.Policies.Where(x => x.DeletedAt == null)];
        public List<DebtColumn> GetDebtsManagements(DebtMonitoringsViewModel input)
        {
            {
                var bulan = input.BulanSelect;
                var tahun = input.TahunSelect;

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
                    ).ToList()
                    .GroupBy(dm => new
                    {
                        PiutangId = dm.PiutangId,
                        IdPelanggan = dm.Piutang!.IdPelanggan,
                        Nama = dm.Piutang.IdPelangganNavigation.Nama,
                        Nomor = dm.Piutang.IdPelangganNavigation.Nomor,
                        Jenis = dm.Piutang.IdPelangganNavigation.JenisNavigation!.Deskripsi,
                        Alamat = dm.Piutang.IdPelangganNavigation.Alamat,
                        Kelurahan = dm.Piutang.IdPelangganNavigation.KelurahanNavigation!.Nama,
                        Kecamatan = dm.Piutang.IdPelangganNavigation.KelurahanNavigation.KodeKecNavigation!.Nama,

                    })
                    .Select(g => new DebtColumn
                    {
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
                        ),
                        RentangWaktu = g.Max(dm =>
                            dm.Pembayaran?.TanggalBayar != null
                                ? dm.Pembayaran.TanggalBayar.Value.ToDateTime(TimeOnly.MinValue)
                                : dm.Tanggal
                        ) is DateTime latestDate
                            ? (int)(DateTime.Now - latestDate).TotalDays
                            : 0
                    })
                    .ToList();
            }
        }
    }
}
