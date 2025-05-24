using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize (Roles ="Administrator Sistem, Pimpinan")]
    public class DebtReportsController : BaseController
    {
        public IActionResult Index()
        {
            DebtReportsService service = new(_context);
            DebtReportsViewModel model = new DebtReportsViewModel
            {
                Debts = service.GetDebts()
            };
            return View(model);
        }
    }
}
