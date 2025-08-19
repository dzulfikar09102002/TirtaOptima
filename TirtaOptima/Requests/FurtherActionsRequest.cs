using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class FurtherActionsRequest(FurtherActionsViewModel input, FurtherActionsService service)
    {
        public FurtherActionsViewModel UserInput { get; set; } = input;
        public FurtherActionsService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (UserInput.BulanSelect <= 0 && UserInput.TahunSelect <= 0)
            {
                ErrorMessage = "Pilih Bulan dan Tahun Terlebih Dahulu";
                return false;
            }
            return true;
        }
    }
}
