using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Models;
using TirtaOptima.Services;

namespace TirtaOptima.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public IActionResult Index()
        {
            UserService service = new(_context);
            User? user = service.GetUser(UserId);
            return View(user);
        }
    }
}
