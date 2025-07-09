using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class FuzzyComparisonViewModel
    {
        public long Kriteria1Id { get; set; }
        public long Kriteria2Id { get; set; }

        public string NamaKriteria1 { get; set; } = "";
        public string NamaKriteria2 { get; set; } = "";

        public decimal L { get; set; }
        public decimal M { get; set; }
        public decimal U { get; set; }
    }

}
