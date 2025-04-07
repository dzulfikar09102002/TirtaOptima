using System.ComponentModel.DataAnnotations;
using TirtaOptima.Models;
using static System.Collections.Specialized.BitVector32;

namespace TirtaOptima.ViewModels
{
    public class AuthViewModel
    {

        [Required(ErrorMessage = "Username harus diisi")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password harus diisi")]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }

    }
}