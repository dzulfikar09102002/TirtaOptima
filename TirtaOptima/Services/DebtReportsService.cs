using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class DebtReportsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Debt> GetDebts() => [.._context.Debts
            .Include(x => x.Pelanggan).ThenInclude(x => x.JenisNavigation)
            .Include(x => x.Pelanggan).ThenInclude(x => x.KelurahanNavigation)
            .ThenInclude(x => x.KodeKecNavigation).Where(x => x.DeletedAt == null)];
    }
}
