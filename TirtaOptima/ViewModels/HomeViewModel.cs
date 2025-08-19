using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class HomeViewModel
    {
        public int CustomerCount { get; set; } 
        public int? DebtCustomers {  get; set; }
        public decimal? DebtSum {  get; set; }
        public int? LetterCount { get; set; }
        public List<DebtPerMonthDto> DebtPerMonth { get; set; } = new();
        public List<CollectionPerMonthDto> CollectionPerMonths { get; set; } = new();
        public List<Debt> Debts { get; set; } = new();
    }
    public class DebtPerMonthDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalNominal { get; set; }
    }
    public class CollectionPerMonthDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public double SuccessRatePerMonth { get; set; }
    }

}
