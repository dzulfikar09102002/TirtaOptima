using Microsoft.AspNetCore.Mvc.ModelBinding;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class BillRequest(BillViewModel input, BillService service)
    {
        public BillViewModel UserInput { get; set; } = input;
        public BillService Service { get; set; } = service;
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
