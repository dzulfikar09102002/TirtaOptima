using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class AuthRequest(AuthViewModel input, AuthService service)
    {
        public AuthViewModel UserInput { get; set; } = input;
        public AuthService Service { get; set; } = service;
        public User? User { get; set; }
        public string? ErrorMassage { get; set; }
        public bool Validate()
        {
            User? user = UserInput.Username != null ? Service.GetUser(UserInput.Username) : null;
            if (UserInput.Username == null || UserInput.Password == null)
            {
                ErrorMassage = "Masukkan Username & Password yang Benar";
                return false;
            }
            else if (user == null)
            {
                ErrorMassage = "Username Anda Salah";
                return false;
            }
            else if (user.DeletedAt != null || user.Status == false)
            {
                ErrorMassage = "Akun Sudah tidak Aktif";
                return false;
            }
            else if (EncryptHelper.GeneratedPassword(user.Username ?? "", UserInput.Password ?? "") != user.Password)
            {
                ErrorMassage = "Password Anda Salah";
                return false;
            }
            else
            {
                User = user;
                return true;
            }

        }
    }
}
