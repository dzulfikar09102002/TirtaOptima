using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
    public class BillsController : BaseController
    {
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-bill")?.ToString(); 
            var tahun = TempData.Peek("Tahun-bill")?.ToString();

            BillViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-bill");
                TempData.Keep("Tahun-bill");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            BillService service = new(_context);
            BillViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            BillRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? throw new Exception();
                    return Json(ResponseBase.Message);
                }
                TempData["Source"] = "database";
                model.Bills = service.GetBills(model);
                var idPelangganList = model.Bills.Select(x => x.IdPelanggan).Distinct().ToList();
                model.Customers = service.GetCustomers(idPelangganList);
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDataApi(string bulan, string tahun)
        {
            BillService service = new(_context);
            BillViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            BillRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? throw new Exception();
                    return Json(ResponseBase);
                }
                TempData["Source"] = "api";
                bool result = await service.GetBillsApiAsync(model);
                if (!result)
                {
                    ResponseBase.Message = service.Message ?? throw new Exception();
                    return Json(ResponseBase);
                }
                model.Bills = service.Bills;
                var idPelangganList = model.Bills.Select(x => x.IdPelanggan).Distinct().ToList();
                model.Customers = service.GetCustomers(idPelangganList);
                if (result && !string.IsNullOrEmpty(service.Message))
                {
                    ViewBag.Message = service.Message;
                }
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? "Terjadi Kesalahan";
                return Json(ResponseBase);
            }
        }

        [HttpPost]
        public IActionResult Save(string[] selectedbills)
        {
            try
            {
                BillService service = new(_context);
                service.Store(selectedbills, UserId);
                ResponseBase.Message = "Rekening berhasil dicatat";
                ResponseBase.Status = StatusEnum.Success;
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }
            return Json(ResponseBase);
        }
    }
}
