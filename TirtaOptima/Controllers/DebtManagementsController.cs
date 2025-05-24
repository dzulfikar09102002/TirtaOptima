using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem, Petugas Pengelola Piutang")]
    public class DebtManagementsController : BaseController
    {
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-debt")?.ToString();
            var tahun = TempData.Peek("Tahun-debt")?.ToString();

            DebtManagementsViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-debt");
                TempData.Keep("Tahun-debt");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            DebtManagementsService service = new(_context);
            DebtManagementsViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            DebtManagementsRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    throw new Exception(ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan");
                }
                model.DebtManagements = service.GetDebtSummaries(model);
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }
        }
        [HttpGet]
        public IActionResult Detail(long id)
        {
            var bulan = TempData.Peek("Bulan-debt")?.ToString();
            var tahun = TempData.Peek("Tahun-debt")?.ToString();
            if (bulan == null || tahun == null)
            {
                return RedirectToAction("Index");
            }
            DebtManagementsService service = new(_context);
            CustomerService customerService = new(_context);
            Customer? customer = customerService.GetCustomer(id);
            if (customer == null)
            {
                NotFound();
            }
            DebtManagementsViewModel model = new()
            {
                IdPelanggan = id,
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun),
            };
            model.DebtManagements = service.GetDebtManagement(model);
            return View("Detail", model);
        }
        [HttpPost]
        public IActionResult Correction(DebtManagementsViewModel input)
        {
            DebtManagementsService service = new(_context);
            DebtManagementsViewModel model = new DebtManagementsViewModel
            {
                Pencatatan = DateOnly.FromDateTime(DateTime.Now),
                BulanSelect = input.BulanSelect,
                TahunSelect = input.TahunSelect

            };
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult Note(DebtManagementsViewModel input)
        {
            try
            {
                DebtManagementsService service = new(_context);
                DebtManagementsRequest requestValidator = new(input, service);
                if (!requestValidator.ValidateInput())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan";
                    throw new Exception(ResponseBase.Message);
                };
                service.Note(input, UserId);
                ResponseBase.Status = StatusEnum.Success;
                ResponseBase.Message = "Data Berhasil Ditambahkan";
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message;
            }
            return Json(ResponseBase);
        }
    }
}
