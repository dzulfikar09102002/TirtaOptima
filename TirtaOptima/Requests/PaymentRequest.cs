using Microsoft.AspNetCore.Mvc.ModelBinding;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class PaymentRequest(ModelStateDictionary modelState, PaymentViewModel input, PaymentService service)
    {
        public ModelStateDictionary ModelState { get; set; } = modelState;
        public PaymentViewModel UserInput { get; set; } = input;
        public PaymentService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (UserInput.BulanSelect <= 0 && UserInput.TahunSelect <= 0)
            {
                ErrorMessage = "Pilih Bulan dan Tahun Terlebih Dahulu";
                return false;
            }
            return ModelState.IsValid;
        }
    }
}
