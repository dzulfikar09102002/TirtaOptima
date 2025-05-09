using System.Security.Claims;
using TirtaOptima.Services;

namespace TirtaOptima.ViewModels
{
    public class NavbarViewModel
    {
        public NavbarViewModel(ClaimsPrincipal Cookie)
        {
            NavbarService service = new(Cookie);
            Name = service.GetName();
            Role = service.GetRole();
            Email = service.GetEmail();
            ProfilePicture = service.GetProfilePicture();
        }

        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
    }
}
