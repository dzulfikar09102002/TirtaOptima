using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class StrategyResultViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public List<Debt> Debts { get; set; } = new();
        public List<ResultColumn> Results { get; set; } = new();
        public List<AssignColumn> SelectedItems { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public long? Penagih { get; set; }
    }
    public class AssignColumn
    {
        public long Id { get; set; }
        public long? Policy { get; set; }
    }
    public class ResultColumn
    {
        public long Id { get; set; }
        public long PiutangId { get; set; }
        public string? Pelanggan { get; set; }
        public decimal Nominal { get; set; }
        public decimal Umur { get; set; }
        public decimal Jenis { get; set; }
        public decimal Status { get; set; }
        public decimal Layanan { get; set; }
        public decimal Alamat { get; set; }
        public decimal Score { get; set; }
        public string? Tindakan { get; set; }
        public int ZonaGroup { get; set; }
    }

}
