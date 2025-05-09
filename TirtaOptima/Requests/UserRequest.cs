using TirtaOptima.Services;
using TirtaOptima.ViewModels;
using SixLabors.ImageSharp;
namespace TirtaOptima.Requests
{
    public class UserRequest(UserViewModel input, UserService service)
    {
        public UserViewModel UserInput { get; set; } = input;
        public UserService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(UserInput.NikNip))
            {
                ErrorMessage = "Nik / Nip harus diisi";
                return false;
            }
            if (string.IsNullOrEmpty(UserInput.Name))
            {
                ErrorMessage = "Nama harus diisi";
                return false;
            }
            if (string.IsNullOrEmpty(UserInput.Username))
            {
                ErrorMessage = "Username harus diisi";
                return false;
            }
            string inputNormalized = UserInput.Name.ToLower().Replace(" ", "");
            string usernameNormalized = UserInput.Username.ToLower().Replace(" ", "");
            var existing = Service.GetUsers();
            if (UserInput?.Id > 0 || UserInput?.Id == null)
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool nameExists = existing.Any(x =>
                (x.Name ?? "").ToLower().Replace(" ", "") == inputNormalized);
            bool userExists = existing.Any(x =>
            (x.NikNip ?? "").ToLower().Replace(" ", "") == UserInput?.NikNip);
            bool usernameExists = existing.Any(x =>
            (x.Username ?? "").ToLower().Replace(" ", "") == usernameNormalized);

            if (nameExists || userExists || usernameExists)
            {
                ErrorMessage = "Pengguna Sudah Ada";
                return false;
            }
            if (UserInput?.Id == 0)
            {
                if (string.IsNullOrEmpty(UserInput?.Password))
                {
                    ErrorMessage = "Password harus diisi";
                    return false;
                }
                if (string.IsNullOrEmpty(UserInput.ConfirmPassword))
                {
                    ErrorMessage = "Konfirmasi password harus diisi";
                    return false;
                }
                if (UserInput.Password != UserInput.ConfirmPassword)
                {
                    ErrorMessage = "Konformasi password tidak sesuai";
                    return false;
                }
            }
            if (UserInput?.Gender == null)
            {
                ErrorMessage = "Jenis kelamin harus dipilih";
                return false;
            }
            if (string.IsNullOrEmpty(UserInput.Jabatan))
            {
                ErrorMessage = "Jabatan harus diisi";
                return false;
            }
            if (UserInput.RoleId == null || UserInput.RoleId <= 0)
            {       
                ErrorMessage = "Role harus dipilih";
                return false;
            }
            if (UserInput.Img != null)
            {
                using (var image = SixLabors.ImageSharp.Image.Load(UserInput.Img.OpenReadStream()))
                if (image.Width != image.Height)
                {
                    ErrorMessage = "Gambar harus memiliki rasio 1:1";
                    return false;
                }
            }
            return true;
        }

    }
}
