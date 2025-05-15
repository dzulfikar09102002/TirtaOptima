using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class DebtMonitoringsRequest(DebtMonitoringsViewModel input, DebtMonitoringsService service)
    {
        public DebtMonitoringsViewModel UserInput { get; set; } = input;
        public DebtMonitoringsService Service { get; set; } = service;
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
