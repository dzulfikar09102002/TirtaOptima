using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class LetterIssuancesService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;

        public List<Debt> GetDebts(LetterIssuancesViewModel input)
        {
            ViewDebtsHelper.UpdateDebts(_context);
            return
            [.._context.Debts
            .Include(x => x.Pelanggan)
            .ThenInclude(x => x.JenisNavigation)
            .Include(x => x.Pelanggan)
            .ThenInclude(x => x.KelurahanNavigation!)
            .ThenInclude(x =>x.KodeKecNavigation)
            .Include(x => x.Collections)            
            .Where(x => x.DeletedAt == null 
            && x.TanggalTerakhir!.Value.Month == input.BulanSelect
            && x.TanggalTerakhir.Value.Year == input.TahunSelect)];
        }
    }
}
