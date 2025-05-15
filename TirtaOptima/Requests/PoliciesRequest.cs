using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class PoliciesRequest(PoliciesViewModel input, PoliciesService service)
    {
        public PoliciesViewModel UserInput { get; set; } = input;
        public PoliciesService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (string.IsNullOrEmpty(UserInput.NamaStrategi))
            {
                ErrorMessage = "Harap Masukkan Nama Kebijakan";
                return false;
            }
            if (string.IsNullOrEmpty(UserInput.Deskripsi))
            {
                ErrorMessage = "Harap Masukkan Deskripsi";
                return false;
            }
            if (string.IsNullOrEmpty(UserInput.Kondisi))
            {
                ErrorMessage = "Harap Masukkan Konsisi";
                return false;
            }
            if (UserInput.RentangWaktu <=0 || UserInput.RentangWaktu == null)
            {
                ErrorMessage = "Harap Masukkan Rentang Waktu";
                return false;
            }
            string inputNormalized = UserInput.NamaStrategi.ToLower().Replace(" ", "");
            var existing = Service.GetPolicies();
            if (UserInput?.Id > 0 || UserInput?.Id == null)
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool nameExists = existing.Any(x =>
                (x.NamaStrategi ?? "").ToLower().Replace(" ", "") == inputNormalized
            );

            if (nameExists)
            {
                ErrorMessage = "Kebijakan Sudah Ada";
                return false;
            }

            return true;
        }

    }
}
