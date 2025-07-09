using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class StrategyCalculationViewModel
    {
        public List<FuzzyComparisonViewModel> FuzzyComparisons { get; set; } = new();
        public List<Criteria> Criterias { get; set; } = new();
        public CriteriaComparison? CriteriaComparison { get; set; }
        public decimal? ScaleValue { get; set; }
        public long CriteriaId1 { get; set; }
        public long CriteriaId2 { get; set; }
        public bool IsRepricoral { get; set; } = false;
        public string Option { get; set; } = "medium";
        public List<string> CriteriaNames { get; set; } = new();
        public List<NormalisasiDetailViewModel> NormalisasiDetails { get; set; } = new();
        public List<PsoLogViewModel> PsoLogs { get; set; } = new();

    }
}
