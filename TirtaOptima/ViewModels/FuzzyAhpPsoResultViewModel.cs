namespace TirtaOptima.ViewModels
{
    public class FuzzyAhpPsoResultViewModel
    {
        public List<string> CriteriaNames { get; set; } = new();
        public List<decimal> GeomL { get; set; } = new();
        public List<decimal> GeomM { get; set; } = new();
        public List<decimal> GeomU { get; set; } = new();
        public List<decimal> FahpWeights { get; set; } = new();
        public List<decimal> PsoWeights { get; set; } = new();
        public decimal SpearmanCorrelation { get; set; }
    }
}
