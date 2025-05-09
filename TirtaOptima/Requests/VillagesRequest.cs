using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class VillagesRequest(VillagesViewModel input, VillagesService service)
    {
        public VillagesViewModel UserInput { get; set; } = input;
        public VillagesService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (string.IsNullOrEmpty(UserInput.Nama))
            {
                ErrorMessage = "Harap masukkan nama kelurahan";
                return false;
            }
            if (UserInput?.KodeKec <= 0 || UserInput?.KodeKec == null)
            {
                ErrorMessage = "Harap pilih kecamatan";
                return false;
            }
            if(UserInput.Layanan <=0 || UserInput.Layanan == null)
            {
                ErrorMessage = "Harap pilih kualitas layanan";
                return false;
            }
            if(UserInput.Jarak <=0 || UserInput.Jarak == null)
            {
                ErrorMessage = "Harap masukkan jarak";
                return false;
            }
            string inputNormalized = UserInput.Nama.ToLower().Replace(" ", "");
            var existing = Service.GetVillages();
            if (UserInput?.Id > 0 || UserInput?.Id == null)
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool nameExists = existing.Any(x =>
                (x.Nama ?? "").ToLower().Replace(" ", "") == inputNormalized
            );

            if (nameExists)
            {
                ErrorMessage = "Kelurahan sudah ada";
                return false;
            }

            return true;
        }
    }
}
