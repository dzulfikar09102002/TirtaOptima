using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class RoleRequest(RoleViewModel input, RoleService service)
    {
        public RoleViewModel UserInput { get; set; } = input;
        public RoleService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (string.IsNullOrEmpty(UserInput.Name))
            {
                ErrorMessage = "Harap Masukkan Nama Role";
                return false;
            }
            string inputNormalized = UserInput.Name.ToLower().Replace(" ", "");
            var existing = Service.GetRoles();
            if (UserInput?.Id > 0 || UserInput?.Id == null)
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool nameExists = existing.Any(x =>
                (x.Name ?? "").ToLower().Replace(" ", "") == inputNormalized
            );

            if (nameExists)
            {
                ErrorMessage = "Role Sudah Ada";
                return false;
            }

            return true;
        }

    }
}
