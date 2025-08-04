using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class AuthRequest
    {
        public AuthViewModel UserInput { get; set; }
        public AuthService Service { get; set; }
        public User? User { get; set; }
        public string? ErrorMassage { get; set; }

        public AuthRequest(AuthViewModel input, AuthService service)
        {
            UserInput = input;
            Service = service;
        }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(UserInput.Username) || string.IsNullOrWhiteSpace(UserInput.Password))
            {
                ErrorMassage = "Masukkan Username & Password yang Benar";
                return false;
            }

            User = Service.GetUser(UserInput.Username);
            if (User == null)
            {
                ErrorMassage = "Username Anda Salah";
                return false;
            }

            if (User.DeletedAt != null || User.Status == false)
            {
                ErrorMassage = "Akun Sudah Tidak Aktif";
                return false;
            }

            if (User.Lockoutend.HasValue && User.Lockoutend > DateTime.Now)
            {
                var sisa = User.Lockoutend.Value - DateTime.Now;
                ErrorMassage = $"Akun Anda terkunci. Coba lagi dalam {sisa.Minutes} menit.";
                return false;
            }

            var passwordMatch = EncryptHelper.GeneratedPassword(User.Username ?? "", UserInput.Password ?? "") == User.Password;

            if (!passwordMatch)
            {
                User.Failedloginattempts++;

                if (User.Failedloginattempts >= 3)
                {
                    User.Lockoutend = DateTime.Now.AddMinutes(5);
                    ErrorMassage = "Terlalu banyak percobaan login. Akun Anda dikunci selama 5 menit.";
                }
                else
                {
                    ErrorMassage = "Password Anda Salah";
                }

                Service.UpdateUser(User); 
                return false;
            }

            User.Failedloginattempts = 0;
            User.Lockoutend = null;
            Service.UpdateUser(User);

            return true;
        }
    }

}
