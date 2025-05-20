using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize]
    public class LetterIssuancesController : BaseController
    {
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-li")?.ToString();
            var tahun = TempData.Peek("Tahun-li")?.ToString();

            LetterIssuancesViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-li");
                TempData.Keep("Tahun-li");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            LetterIssuancesService service = new(_context);
            LetterIssuancesViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            LetterIssuancesRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    throw new Exception(ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan");
                }
              /*  model.Collections = service.GetCollections(model)*/;
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
