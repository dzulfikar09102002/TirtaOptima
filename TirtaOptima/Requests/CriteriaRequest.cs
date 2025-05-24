using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class CriteriaRequest(CriteriaViewModel input, CriteriaService service)
    {
        public CriteriaViewModel UserInput { get; set; } = input;
        public CriteriaService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if(string.IsNullOrEmpty(UserInput.Name))
            {
                ErrorMessage = "Nama kriteria harus diisi";
                return false;
            }
            string inputNormalized = UserInput.Name.ToLower().Replace(" ", "");
            var existing = Service.GetCriterias();
            if (UserInput?.Id > 0 || UserInput?.Id == null)
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool nameExists = existing.Any(x =>
                (x.Name ?? "").ToLower().Replace(" ", "") == inputNormalized
            );

            if (nameExists)
            {
                ErrorMessage = "Kriteria sudah ada";
                return false;
            }
            return true;
        }
    }
}
