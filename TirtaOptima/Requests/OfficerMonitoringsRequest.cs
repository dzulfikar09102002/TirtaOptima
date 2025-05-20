using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class OfficerMonitoringsRequest(OfficerMonitoringsService service, OfficerMonitoringsViewModel input)
    {
        public OfficerMonitoringsService Service = service;

        public OfficerMonitoringsViewModel UserInput = input;
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
