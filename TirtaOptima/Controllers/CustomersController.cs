using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
    public class CustomersController : BaseController
    {

        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-cs")?.ToString();
            var tahun = TempData.Peek("Tahun-cs")?.ToString();

            var model = new CustomerViewModel();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-cs");
                TempData.Keep("Tahun-cs");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            CustomerService service = new(_context);
            CustomerViewModel model = new CustomerViewModel
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun),
                Kelurahans = service.GetVillages(),
                Kecamatans = service.GetDistricts(),
                Jenis = service.GetCustomerTypes(),
                Status = service.GetStatus(),
            };
            CustomerRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? throw new Exception();
                    return Json(ResponseBase.Message);
                }
                TempData["Source"] = "database";
                model.Customers = service.GetCustomers(model);
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
            CustomerService service = new(_context);
            CustomerViewModel model = new CustomerViewModel
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun),
                Kelurahans = service.GetVillages(),
                Kecamatans = service.GetDistricts(),
                Jenis = service.GetCustomerTypes(),
                Status = service.GetStatus(),
            };
            CustomerRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? throw new Exception();
                    return Json(ResponseBase);
                }
                TempData["Source"] = "api";
                bool result = await service.GetCustomersApiAsync(model);
                if (!result)
                {
                    ResponseBase.Message = service.Message ?? throw new Exception();
                    return Json(ResponseBase);
                }
                model.Customers = service.Customers;
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? "Terjadi Kesalahan";
                return Json(ResponseBase);
            }
        }
        [HttpPost]
        public IActionResult Save(string[] selectedcustomers)
        {
            try
            {
                CustomerService service = new(_context);
                service.Store(selectedcustomers, UserId);
                ResponseBase.Message = "Pelanggan berhasil dicatat";
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
