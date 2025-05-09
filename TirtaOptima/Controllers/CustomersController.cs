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
    public class CustomersController : BaseController
    {

        [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
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

        [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            CustomerService service = new(_context);
            CustomerViewModel model = new CustomerViewModel
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            CustomerRequest requestValidator = new(ModelState, model, service);
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
        [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
        [HttpGet]
        public async Task<IActionResult> GetDataApi(string bulan, string tahun)
        {
            CustomerService service = new(_context);
            CustomerViewModel model = new CustomerViewModel
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            CustomerRequest requestValidator = new(ModelState, model, service);
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
    }
}
