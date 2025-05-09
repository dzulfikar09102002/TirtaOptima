using Microsoft.AspNetCore.Mvc;
using TirtaOptima.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using TirtaOptima.Services;
using TirtaOptima.Requests;

namespace TirtaOptima.Controllers;
public class AuthController : BaseController
{

    public IActionResult Index() => (Cookie.Identity?.IsAuthenticated == true) ? RedirectToAction("Index", "Home") : View(new AuthViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(AuthViewModel input)
    {
        AuthService service = new(_context);
        AuthRequest requestValidator = new(input, service);

        if (!requestValidator.Validate())
        {
            TempData["ErrorMessages"] = requestValidator.ErrorMassage;
            return View("Index", input);
        }
        if (requestValidator.User != null &&
                requestValidator.User.Username != null &&
                requestValidator.User.Email != null &&
                requestValidator.User.Role != null &&
                requestValidator.User.Role.Name != null)
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, requestValidator.User.Name),
                 new Claim(ClaimTypes.Email, requestValidator.User.Email),
                 new Claim(ClaimTypes.Role, requestValidator.User.Role.Name),
                 new Claim("UserId", requestValidator.User.Id.ToString()),
                 new Claim("RoleId", requestValidator.User.RoleId.ToString() ?? ""),
             };

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (input.ReturnUrl != null)
            {
                return Redirect(input.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        TempData["ErrorMessages"] = "Terjadi Kesalahan pada saat Login";
        return View("Index", input);
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Auth");
    }
}