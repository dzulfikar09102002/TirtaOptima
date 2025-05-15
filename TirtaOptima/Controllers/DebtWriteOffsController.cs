using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    public class DebtWriteOffsController : BaseController
    {
        public IActionResult Index()
        {

            var bulan = TempData.Peek("Bulan-dwo")?.ToString();
            var tahun = TempData.Peek("Tahun-dwo")?.ToString();

            DebtWriteOffsViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-dwo");
                TempData.Keep("Tahun-dwo");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            DebtWriteOffsService service = new(_context);
            DebtWriteOffsViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            DebtWriteOffsRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    throw new Exception(ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan");
                }
                model.Debts = service.GetDebtSummaries(model);
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DebtWriteOffsViewModel input)
        {
            try
            {
                DebtWriteOffsService service = new(_context);
                if (service.GetDebt(input) == null)
                {
                    return NotFound();
                }
                service.Delete(input, UserId);
                ResponseBase.Message = "Data Berhasil Dihapus";
                ResponseBase.Status = StatusEnum.Success;
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message;
            }
            return Json(ResponseBase);
        }
    }

}
