using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class DebtMonitoringsViewModel
    {
        public List<Debt> Debts { get; set; } = new();
        public List<Policy> Policies { get; set; } = new();
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }

    }
}
