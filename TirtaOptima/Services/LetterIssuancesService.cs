using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class LetterIssuancesService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;

        public List<Debt> GetDebts() => [.. _context.Debts.Include(x => x.Pelanggan)];
    }
}
