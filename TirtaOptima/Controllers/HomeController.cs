using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TirtaOptima.Models;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeService service = new(_context);
            HomeViewModel model = new HomeViewModel
            {
                CustomerCount = service.GetCustomers().Where(x => x.Status == 2).Count(),
                DebtCustomers = service.GetCustomers()
                                 .Count(x => x.Debts.Any()),
                DebtSum = service.GetDebts().Sum(x => x.Nominal),
                LetterCount = service.GetLetters().Count,
                DebtPerMonth = service.GetDebtPerMonthByYear(),
                CollectionPerMonths = service.GetCollectionPerMonths(),
                Debts = service.GetDebts().OrderByDescending(x => x.Nominal).Take(5).ToList(),
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
