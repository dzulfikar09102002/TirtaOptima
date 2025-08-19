using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class FurtherActionsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Collection> GetCollections(FurtherActionsViewModel input, long userid)
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
    }
}
