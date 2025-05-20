using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    public class LetterDeliveriesController : BaseController
    {
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-ld")?.ToString();
            var tahun = TempData.Peek("Tahun-ld")?.ToString();

            LetterDeliveriesViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-ld");
                TempData.Keep("Tahun-ld");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            CollectionMonitoringsService service = new(_context);
            CollectionMonitoringsViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            CollectionMonitoringsRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    throw new Exception(ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan");
                }
                model.Collections = service.GetCollections(model);
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
