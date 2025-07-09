using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class StrategyCalculationRequest(StrategyCalculationViewModel input, StrategyCalculationService service)
    {
        public StrategyCalculationViewModel UserInput { get; set; } = input;
        public StrategyCalculationService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (UserInput.CriteriaId1 <= 0 || UserInput.CriteriaId2 <= 0)
            {
                ErrorMessage = "Tidak ada perbandingan kriteria yang dipilih";
                return false;
            }
            if (UserInput.ScaleValue <= 0)
            {
                ErrorMessage = "Harap pilih nilai perbandingan";
                return false;
            }
            return true;
        }
    }
}
