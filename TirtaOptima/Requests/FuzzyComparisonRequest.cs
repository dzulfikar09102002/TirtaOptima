using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class FuzzyComparisonRequest
    {
        public List<FuzzyComparisonViewModel> Comparisons { get; set; } = new();
        public int CriteriaCount { get; set; }
    }

}
