using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem, Petugas Penagihan")]
    public class OfficerMonitoringsController : BaseController
    {
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-mo")?.ToString();
            var tahun = TempData.Peek("Tahun-mo")?.ToString();

            OfficerMonitoringsViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-mo");
                TempData.Keep("Tahun-mo");
            }
            return View(model);
        }
    }
}