using Microsoft.AspNetCore.Mvc.ModelBinding;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class CustomerTypeRequest(CustomerTypeViewModel input, CustomerTypeService service)
    {
        public CustomerTypeViewModel UserInput { get; set; } = input;
        public CustomerTypeService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (string.IsNullOrEmpty(UserInput.Id))
            {
                ErrorMessage = "Harap Masukkan Jenis Pelanggan";
                return false;
            }
            if (string.IsNullOrEmpty(UserInput.Deskripsi))
            {
                ErrorMessage = "Harap Masukkan Deskripsi Pelanggan";
                return false;
            }
            if(UserInput.MinPakai == null)
            {
				ErrorMessage = "Harap Masukkan Minimal Pemakaian";
				return false;
			}
            if(UserInput.Tarif1 == null)
            {
				ErrorMessage = "Harap Masukkan Tarif 1";
				return false;
			}
            string descNormalized = UserInput.Deskripsi.ToLower().Replace(" ", "");

            var existing = Service.GetCustomerTypes();
            if(!string.IsNullOrEmpty(UserInput?.Id))
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool descExists = existing.Any(x =>
                (x.Deskripsi ?? "").ToLower().Replace(" ", "") == descNormalized
            );

            if (descExists )
            {
                ErrorMessage = "Jenis Pelanggan Sudah Ada";
                return false;
            }

            return true;
        }

    }
}
