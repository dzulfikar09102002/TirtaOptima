using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class NavbarService
    {
        private readonly DatabaseContext _context;
        private readonly User _currentUser;
        private readonly long _roleId;
        public NavbarService(ClaimsPrincipal Cookie)
        {
            _context = new DatabaseContext();
            long UserId = Convert.ToInt64(Cookie.FindFirst("UserId")?.Value);
            _roleId = Convert.ToInt64(Cookie.FindFirst("RoleId")?.Value);
            _currentUser = _context.Users.Include(role => role.Role).First(u => u.Id == UserId);
        }

        public string GetName()
        {
            return _currentUser.Name;
        }
        public string GetRole()
        {
            return _currentUser?.Role?.Name ?? "";
        }
        public string GetEmail()
        {
            return _currentUser?.Email ?? "";
        }
        public string GetProfilePicture()
        {
            string photo = _currentUser?.Photo ?? "user-dummy-img.jpg";
            return photo;
        }
    }
}
