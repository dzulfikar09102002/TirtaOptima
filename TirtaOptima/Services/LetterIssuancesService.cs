using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class LetterIssuancesService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Collection> GetCollections(LetterIssuancesViewModel input, long userid)
        {
            bool isAdmin = _context.Roles
                .Where(r => r.Id == 1 && r.DeletedAt == null)
                .Include(r => r.Users)
                .Any(r => r.Users.Any(u => u.Id == userid));

            return _context.Collections
                .Include(x => x.Piutang)
                    .ThenInclude(x => x.StrategyResults)
                .Include(x => x.Tindakan)
                .Include(x => x.Piutang)
                    .ThenInclude(x => x.Pelanggan)
                        .ThenInclude(p => p.JenisNavigation)
                .Include(x => x.Piutang)
                    .ThenInclude(x => x.Pelanggan)
                        .ThenInclude(p => p.KelurahanNavigation)
                            .ThenInclude(kel => kel!.KodeKecNavigation)
                .Where(x => x.DeletedAt == null
                    && (isAdmin || x.PenagihId == userid)
                    && x.CreatedAt.Month == input.BulanSelect
                    && x.CreatedAt.Year == input.TahunSelect)
                .ToList();
        }
        public bool IsExist(long id)
        {
            return _context.Collections
                .Any(x => x.DeletedAt == null && x.Id == id);
        }
        public Letter? GetLetter(long id) => _context.Letters
            .Include(x => x.Kategori)
            .Include(x => x.Penagihan)
                .ThenInclude(x => x.Piutang)
                    .ThenInclude(x => x.Pelanggan)
                        .ThenInclude(x => x.KelurahanNavigation!)
                            .ThenInclude(x => x.KodeKecNavigation)
            .Include(x => x.Penagihan)
                .ThenInclude(x => x.Piutang)
                    .ThenInclude(x => x.Pelanggan)
                        .ThenInclude(x => x.JenisNavigation)
            .Include(x => x.Penagihan).ThenInclude(x => x.Piutang)
                   .ThenInclude(x => x.Pelanggan)
                        .ThenInclude(x => x.StatusNavigation)
            .Include(x => x.Tindakan)
            .Include(x => x.Penagihan)
                .ThenInclude(x => x.Penagih)
            .Include(x => x.Pimpinan)
                .ThenInclude(x => x.User)
           .FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public List<Letter> GetLetters(long id) => [.._context.Letters.Include(x => x.Kategori).Where(x => x.DeletedAt == null && x.PenagihanId == id)];
        public Collection? GetCollection(long id)
        {
            return _context.Collections
                .Include(x => x.Piutang)
                    .ThenInclude(x => x.StrategyResults)
                .Include(x => x.Tindakan)
                .Include(x => x.Piutang)
                    .ThenInclude(x => x.Pelanggan)
                        .ThenInclude(p => p.JenisNavigation)
                .Include(x => x.Piutang)
                    .ThenInclude(x => x.Pelanggan)
                        .ThenInclude(p => p.KelurahanNavigation)
                            .ThenInclude(kel => kel!.KodeKecNavigation)
                .FirstOrDefault(x => x.DeletedAt == null && x.Id == id);

        }

        public List<LetterCategory> GetLetterCategories() => [.. _context.LetterCategories.Where(x => x.DeletedAt == null)];
        public List<Leader> GetLeaders() => [.. _context.Leaders.Include(x => x.User).Where(x => x.DeletedAt == null)];
        public List<DebtsSaldoPerMonthDto> GetSaldoPerMonth(long id)
        {
            var result = _context.DebtsManagements
                .Include(x => x.Piutang)
                .Include(x => x.Pembayaran)
                .Where(x =>
                    (x.Piutang!.IdPelanggan == id || x.Pembayaran!.IdPelanggan == id)
                    && x.DeletedAt == null
                )
                .Select(x => new
                {
                    Month = (x.Piutang != null ? x.Piutang.Bulan : x.Pembayaran!.Bulan),
                    Year = (x.Piutang != null ? x.Piutang.Tahun : x.Pembayaran!.Tahun),
                    Nominal = x.Nominal,
                    Status = x.Status
                })
                .GroupBy(x => new { x.Year, x.Month })
                .Select(g => new DebtsSaldoPerMonthDto
                {
                    Year = g.Key.Year ?? 0,
                    Month = g.Key.Month ?? 0,
                    TotalKredit = g.Where(i => i.Status == "Kredit").Sum(i => i.Nominal),
                    TotalDebit = g.Where(i => i.Status == "Debit").Sum(i => i.Nominal)
                })
                .Where(x => (x.TotalKredit - x.TotalDebit) > 0) // masih ada saldo utang
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            return result;
        }

        public void Create(LetterIssuancesViewModel input, long userid)
        {
            if (input.Collection != null)
            {
                var idsurat = _context.LetterCategories.FirstOrDefault(x => x.Id == input.KategoriId)?.Code;
                var lastId = _context.Letters.Any()
                ? _context.Collections.Max(x => x.Id) + 1
                : 1;
                var nomorsurat = idsurat + "/" + lastId + "/" + "423.500." + DateTime.Now.Month + "/" + DateTime.Now.Year;
                _context.Letters.Add(new Letter
                {
                    Id = lastId,
                    PenagihanId = input.Collection.Id,
                    TindakanId = input.Collection.TindakanId ?? 1,
                    PimpinanId = input.PimpinanId,
                    Sifat = input.Sifat,
                    Lampiran = input.Lampiran ?? 0,
                    Ket = input.Ket,
                    Note = input.Note,
                    NomorSurat = nomorsurat,
                    KategoriId = input.KategoriId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userid,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = userid

                });
                _context.SaveChanges();
            }
        }
    }
}
