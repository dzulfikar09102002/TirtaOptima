using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class CollectionMonitoringsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Collection> GetCollections(CollectionMonitoringsViewModel input)
        {
            ViewDebtsHelper.UpdateDebts(_context);
            return 
            [.. _context.Collections.Where(x => x.DeletedAt == null && 
            x.Tanggal.Month == input.BulanSelect && x.Tanggal.Year == input.TahunSelect)
            .Include(x => x.Piutang)
            .ThenInclude(x => x.Pelanggan)
            .ThenInclude(x => x.KelurahanNavigation!)
            .ThenInclude(x => x.KodeKecNavigation)
            .Include(x => x.Penagih)
            .Include(x => x.Tindakan)];
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
           .FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public List<Letter> GetLetters(long id) => [.. _context.Letters
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
            .Where(x => x.DeletedAt == null && x.PenagihanId == id)];
    }
}
