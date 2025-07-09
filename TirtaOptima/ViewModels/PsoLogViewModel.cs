namespace TirtaOptima.ViewModels
{
    public class PsoLogViewModel
    {
        public int Iteration { get; set; }
        public List<decimal> Weights { get; set; } = new();
        public decimal Fitness { get; set; }
        public List<decimal> Velocity { get; set; } = new();
        public List<decimal> PBest { get; set; } = new();
        public decimal PBestScore { get; set; }
    }
}
