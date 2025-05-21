using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Requests;
using TirtaOptima.Services;
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
        [Authorize(Roles = "Administrator Sistem, Petugas Penagihan")]
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            OfficerMonitoringsService service = new(_context);
            OfficerMonitoringsViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            OfficerMonitoringsRequest requestValidator = new(service, model);
            try
            {
                if (!requestValidator.Validate())
                {
                    throw new Exception(ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan");
                }
                model.Officers = service.GetOfficers();
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }   
        }
        [HttpGet]
        public IActionResult Detail(OfficerMonitoringsViewModel input)
        {
            OfficerMonitoringsService service = new(_context);
            input.Collections = service.GetCollections(input).Where(x => x.PenagihId == input.IdPenagih).ToList();
            if (input.Collections == null)
            {
                return NotFound();
            }
            return View(input);
        }
    }
}