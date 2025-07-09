using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class LetterIssuancesViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public List<Debt> Debts { get; set; } = new();
        public Debt? Debt { get; set; }
    }
}
