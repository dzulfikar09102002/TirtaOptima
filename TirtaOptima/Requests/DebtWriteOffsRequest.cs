using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class DebtWriteOffsRequest(DebtWriteOffsViewModel input, DebtWriteOffsService service)
    {
        public DebtWriteOffsViewModel UserInput { get; set; } = input;
        public DebtWriteOffsService Service { get; set; } = service;
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
