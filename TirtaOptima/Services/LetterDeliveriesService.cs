using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class LetterDeliveriesService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Collection> GetCollections(LetterDeliveriesViewModel input, long userid)
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
                .Include(x => x.Penagih)
                .Include(x => x.Letters)
                .Include(x => x.Tindakan)
                .FirstOrDefault(x => x.DeletedAt == null && x.Id == id);

        }
        public List<Letter> GetLetters(long id) => [.. _context.Letters.Include(x => x.Kategori).Where(x => x.DeletedAt == null && x.PenagihanId == id)];
        public Letter? GetLetter(long id) => _context.Letters
            .Include(x => x.Kategori)
            .Include(x => x.Penagihan)
                .ThenInclude(x => x.Piutang)
                    .ThenInclude(x => x.Pelanggan)
            .Include(x => x.Tindakan)
            .Include(x => x.Penagihan)
                .ThenInclude(x => x.Penagih)
            .FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public void Save(LetterDeliveriesViewModel input, long userid)
        {
            var existingCollection = _context.Collections.FirstOrDefault(x => x.Id == input.Letter!.PenagihanId && x.DeletedAt == null);
            var existingLetter = GetLetter(input.Letter!.Id);
            if (existingCollection != null)
            {
                existingCollection.Ket = input.Letter!.Ket;
                existingCollection.Tanggal = DateTime.Now;
                existingCollection.UpdatedAt = DateTime.Now;
                existingCollection.UpdatedBy = userid;
                if (input.Letter.Status == "Berhasil Ditagih")
                {
                    existingCollection.Status = "Tuntas";
                }
                else
                {
                    existingCollection.Status = "Belum Tuntas";
                }
            }
            if (existingLetter != null && input.Letter.Status != "Belum Disampaikan")
            {
                existingLetter.Status = input.Letter.Status;
                existingLetter.UpdatedAt = DateTime.Now;
                existingLetter.UpdatedBy = userid;
                existingLetter.NamaPenerima = input.Letter?.NamaPenerima;
                existingLetter.NotelpPenerima = input.Letter?.NotelpPenerima;
                existingLetter.RencanaBayar = input.Letter?.RencanaBayar;
                existingLetter.Alasan = input.Letter?.Alasan;
                existingLetter.Ket = input.Letter?.Ket;
                existingLetter.Foto = input.Letter?.Foto;
            }
            _context.SaveChanges();
        }
    }
}
