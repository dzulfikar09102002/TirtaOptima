using TirtaOptima.Requests;

namespace TirtaOptima.Interfaces
{
    public interface IFuzzyAhpPsoService
    {
        Task<List<decimal>> CalculateWeightsAsync(FuzzyComparisonRequest request);
    }
}
