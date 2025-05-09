using System.ComponentModel.DataAnnotations;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class UserViewModel
    {
        public List<User> Users { get; set; } = new();
        public List<Role> Roles { get; set; } = new();
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? More { get; set; }

        public bool? Gender { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
		[Compare("Password", ErrorMessage = "Konfirmasi Password tidak sama dengan Password")]
		public string? ConfirmPassword { get; set; }

        public string? Alamat { get; set; }

        public string? NomorTelepon { get; set; }

        public string? NikNip { get; set; }

        public long? RoleId { get; set; }

        public string? Photo { get; set; }

        public string? Jabatan { get; set; }

        public string? Email { get; set; }

        public bool? Status { get; set; }
        public IFormFile? Img { get; set; }
    }
}
