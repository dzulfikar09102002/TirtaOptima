using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class OfficerMonitoringsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Collection> GetCollections(OfficerMonitoringsViewModel input)
        {
            ViewDebtsService service = new(_context);
            service.UpdateDebts();
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
        public List<User> GetOfficers(OfficerMonitoringsViewModel input)
        {
            ViewDebtsService service = new(_context);
            service.UpdateDebts();
            return [.. _context.Users
        .Where(x => x.RoleId == 3 && x.DeletedAt == null)
        .Include(x => x.CollectionPenagihs).ThenInclude(x => x.Piutang)];
        }
    }
}
