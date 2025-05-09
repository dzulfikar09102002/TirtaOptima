using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    public class PaymentsController : BaseController
    {
        [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-pym")?.ToString();
            var tahun = TempData.Peek("Tahun-pym")?.ToString();

            var model = new PaymentViewModel();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-pym");
                TempData.Keep("Tahun-pym");
            }
            return View(model);
        }
        [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            PaymentService service = new(_context);
            PaymentViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            PaymentRequest requestValidator = new(ModelState, model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? throw new Exception();
                    return Json(ResponseBase.Message);
                }
                TempData["Source"] = "database";
                model.Payments = service.GetPayments(model);
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? "Terjadi Kesalahan";
                return Json(ResponseBase);
            }
        }

        [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
        [HttpGet]
        public async Task<IActionResult> GetDataApi(string bulan, string tahun)
        {
            PaymentService service = new(_context);
            PaymentViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            PaymentRequest requestValidator = new(ModelState, model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? throw new Exception();
                    return Json(ResponseBase);
                }
                TempData["Source"] = "api";
                bool result = await service.GetPaymentsApiAsync(model);
                if (!result)
                {
                    ResponseBase.Message = service.Message ?? throw new Exception();
                    return Json(ResponseBase);
                }
                model.Payments = service.Payments;
                if (result && !string.IsNullOrEmpty(service.Message))
                {
                    ViewBag.Message = service.Message;
                }
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
