using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class HomeService
    {
        private readonly DatabaseContext _context;

        public HomeService(DatabaseContext context)
        {
            _context = context;
            ViewDebtsHelper.UpdateDebts(_context);
        }

        public List<Customer> GetCustomers()
            => [.. _context.Customers.Where(x => x.DeletedAt == null)];

        public List<Debt> GetDebts()
            => [.. _context.Debts.Include(x => x.Pelanggan).Where(x => x.DeletedAt == null)];

        public List<Letter> GetLetters()
            => [.. _context.Letters.Where(x => x.DeletedAt == null)];

        public List<DebtPerMonthDto> GetDebtPerMonthByYear(int year = 2024)
        {
            // Data asli dari DB
            var raw = _context.Debts
                .Where(x => x.DeletedAt == null && x.TanggalTerakhir!.Value.Year == year)
                .GroupBy(x => new { x.TanggalTerakhir!.Value.Year, x.TanggalTerakhir!.Value.Month })
                .Select(g => new DebtPerMonthDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalNominal = g.Sum(d => d.Nominal)
                })
                .ToList();

            // Generate bulan Jan–Des sesuai tahun input
            var result = new List<DebtPerMonthDto>();
            for (int month = 1; month <= 12; month++)
            {
                var existing = raw.FirstOrDefault(x => x.Year == year && x.Month == month);

                result.Add(new DebtPerMonthDto
                {
                    Year = year,
                    Month = month,
                    TotalNominal = existing?.TotalNominal ?? 0
                });
            }

            return result;
        }

        public List<CollectionPerMonthDto> GetCollectionPerMonths(int year = 2025)
        {
            var raw = _context.Collections
                .Where(x => x.DeletedAt == null && x.Tanggal!.Year == year)
                .GroupBy(x => new { x.Tanggal!.Year, x.Tanggal!.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Success = g.Count(c => c.Status == "tuntas"),
                    Count = g.Count()
                })
                .ToList();

            var result = new List<CollectionPerMonthDto>();
            for (int month = 1; month <= 12; month++)
            {
                var existing = raw.FirstOrDefault(x => x.Month == month);
                double rate = 0;
                if (existing != null && existing.Count > 0)
                {
                    rate = (double)existing.Success / existing.Count * 100;
                }

                result.Add(new CollectionPerMonthDto
                {
                    Year = year,
                    Month = month,
                    SuccessRatePerMonth = Math.Round(rate, 2)
                });
            }

            return result;
        }



    }

}
