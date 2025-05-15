using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    public class DebtMonitoringsController : BaseController
    {
        [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-mp")?.ToString();
            var tahun = TempData.Peek("Tahun-mp")?.ToString();

            DebtMonitoringsViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-mp");
                TempData.Keep("Tahun-mp");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            DebtMonitoringsService service = new(_context);
            DebtMonitoringsViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            DebtMonitoringsRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    throw new Exception(ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan");
                }
                model.Debts = service.GetDebtsManagements(model);
                model.Policies = service.GetPolicies();
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }
        }
    }
}
