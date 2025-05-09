using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class DebtManagementsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;

        public List<DebtSummaryViewModel> GetDebtManagement(DebtManagementsViewModel model)
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
                )
                .Select(dm => new DebtSummaryViewModel
                {
                    Id = dm.Id,
                    IdPelanggan = dm.Piutang!.IdPelanggan,
                    NamaPelanggan = dm.Piutang.IdPelangganNavigation.Nama,
                    NomorPelanggan = dm.Piutang.IdPelangganNavigation.Nomor,
                    JenisPelanggan = (dm.Piutang.IdPelangganNavigation.Jenis + " " + dm.Piutang.IdPelangganNavigation.JenisNavigation!.Deskripsi),
                    Alamat = dm.Piutang.IdPelangganNavigation.Alamat,
                    Kelurahan = dm.Piutang.IdPelangganNavigation.KelurahanNavigation!.Nama,
                    Kecamatan = dm.Piutang.IdPelangganNavigation.KelurahanNavigation.KodeKecNavigation!.Nama,
                    Periode = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(bulan)} | {tahun}",
                    TotalNominal = dm.Status == "Kredit" ? -dm.Nominal : dm.Nominal,
                    Status = dm.Piutang.IdPelangganNavigation.StatusNavigation.Name,
                    Pencatatan = dm.Tanggal,
                    Pembayaran = dm.Pembayaran!.TanggalBayar
                })
                .ToList();
        }

        public List<DebtSummaryViewModel> GetDebtSummaries(DebtManagementsViewModel model)
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
    }
}
