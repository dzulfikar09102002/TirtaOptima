using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    public class FurtherActionsController : BaseController
    {
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-fa")?.ToString();
            var tahun = TempData.Peek("Tahun-fa")?.ToString();

            FurtherActionsViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-fa");
                TempData.Keep("Tahun-fa");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            FurtherActionsService service = new(_context);
            FurtherActionsViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            FurtherActionsRequest requestValidator = new(model, service);
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
        public IActionResult Letters(long id)
        {
            LetterDeliveriesService service = new(_context);
            if (!service.IsExist(id))
            {
                return NotFound();
            }
            LetterDeliveriesViewModel model = new LetterDeliveriesViewModel
            {
                Letters = service.GetLetters(id),
            };
            if (model.Collection?.TindakanId > 3)
            {
                return Forbid();
            }
            return PartialView(model);
        }
        public IActionResult Report(long id)
        {
            LetterDeliveriesService service = new(_context);
            LetterDeliveriesViewModel model = new LetterDeliveriesViewModel
            {
                Letter = service.GetLetter(id),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(LetterDeliveriesViewModel input, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            LetterDeliveriesService service = new(_context);
            LetterDeliveriesRequest requestValidador = new(input, service);
            try
            {

                if (!requestValidador.ValidateInput())
                {
                    ResponseBase.Message = requestValidador.ErrorMessage ?? "Terjadi Kesalahan";
                    throw new Exception(ResponseBase.Message);
                }
                var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (input.Img != null)
                {
                    var fileName = $"collection-{input.Letter!.Id}-{timeStamp}{Path.GetExtension(input.Img.FileName)}";
                    var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "assets/images/collections");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        input.Img.CopyTo(stream);
                    }
                    input.Letter!.Foto = $"{fileName}";
                    service.Save(input, UserId);
                    ResponseBase.Message = "Data berhasil disimpan";
                    ResponseBase.Status = StatusEnum.Success;
                }
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
