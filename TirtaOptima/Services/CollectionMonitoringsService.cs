using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class CollectionMonitoringsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Collection> GetCollections(CollectionMonitoringsViewModel input) => 
            [.. _context.Collections.Where(x => x.DeletedAt == null && 
            x.Tanggal.Month == input.BulanSelect && x.Tanggal.Year == input.TahunSelect)
            .Include(x => x.Piutang)
            .ThenInclude(x => x.DebtsManagements)
            .Include(x => x.Piutang)
            .ThenInclude(x => x.IdPelangganNavigation)
            .ThenInclude(x => x.KelurahanNavigation!)
            .ThenInclude(x => x.KodeKecNavigation)
            .Include(x => x.Penagih)
            .Include(x => x.Tindakan)
           ];
        public Collection? GetCollection(long id) => 
            _context.Collections
            .Include(x => x.Piutang)
            .ThenInclude(x => x.IdPelangganNavigation)
            .ThenInclude(x => x.KelurahanNavigation!)
            .ThenInclude(x => x.KodeKecNavigation)
            .Include(x => x.Piutang)
            .ThenInclude(x => x.IdPelangganNavigation)
            .ThenInclude(x => x.JenisNavigation)
            .Include(x => x.Piutang)
            .ThenInclude(x => x.IdPelangganNavigation)
            .ThenInclude(x => x.StatusNavigation)
            .Include(x => x.Penagih)
            .Include(x => x.Tindakan)
            .Include(x => x.Surat)
            .Include(x => x.Piutang)
            .ThenInclude(x => x.DebtsManagements)
            .FirstOrDefault(x => x.DeletedAt == null && x.Id == id);
    }
}
