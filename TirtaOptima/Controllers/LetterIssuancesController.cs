using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem, Petugas Penagihan")]
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
                model.Collections = service.GetCollections(model, UserId);
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }
        }
        [HttpPost]
        public IActionResult Publish(long id)
        {
            LetterIssuancesService service = new(_context);
            if (!service.IsExist(id))
            {
                return NotFound();
            }
            LetterIssuancesViewModel model = new LetterIssuancesViewModel
            {
                Collection = service.GetCollection(id),
                LetterCategories = service.GetLetterCategories(),
                Leaders = service.GetLeaders(),
            };

            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(LetterIssuancesViewModel input)
        {
            try
            {
                LetterIssuancesService service = new(_context);
                if (input.KategoriId <= 0)
                {
                    throw new Exception("Kategori surat harus dipilih");
                }
                service.Create(input, UserId);
                ResponseBase.Status = StatusEnum.Success;
                ResponseBase.Message = "Surat berhasil diterbitkan";
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
            }
            return Json(ResponseBase);
        }
        [HttpPost]
        public IActionResult Letters(long id)
        {
            LetterIssuancesService service = new(_context);
            if (!service.IsExist(id))
            {
                return NotFound();
            }
            LetterIssuancesViewModel model = new LetterIssuancesViewModel
            {
                Letters = service.GetLetters(id)
            };

            return PartialView(model);
        }
        [HttpGet]
        public IActionResult Print(long id)
        {
            LetterIssuancesService service = new(_context);
            Letter? letter = service.GetLetter(id);
            if (letter == null)
            {
                return NotFound();
            }
            LetterIssuancesViewModel model = new LetterIssuancesViewModel
            {
                Letter = letter,
                DebtsManagement = service.GetSaldoPerMonth(letter.Penagihan.Piutang.PelangganId)
               
            };
            return View(model);
        }

    }
}
