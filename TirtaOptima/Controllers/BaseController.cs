using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TirtaOptima.Models;
namespace TirtaOptima.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DatabaseContext _context;

        protected ClaimsPrincipal Cookie => HttpContext.User;

        protected long UserId;

        protected long RoleId;

        public ResponseBase ResponseBase { get; set; } = new();
        public BaseController()
        {
            _context = new DatabaseContext();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            UserId = Convert.ToInt64(Cookie.FindFirst("UserId")?.Value);
            RoleId = Convert.ToInt64(Cookie.FindFirst("RoleId")?.Value);
        }

    }
}
