using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize (Roles ="Administrator Sistem")]
    public class VillagesController : BaseController
    {
        public IActionResult Index()
        {
            VillagesService service = new(_context);
            VillagesViewModel model = new VillagesViewModel
            {
                Villages = service.GetVillages()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Create()
        {
            VillagesService service = new(_context);
            VillagesViewModel model = new VillagesViewModel
            {
                Districts = service.GetDistricts()
            };
            return PartialView("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(VillagesViewModel input)
        {
            try
            {
                VillagesService service = new(_context);
                VillagesRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Village village = ModelHelper.MapProperties<VillagesViewModel, Village>(input);
                service.Store(village, UserId);
                ResponseBase.Status = StatusEnum.Success;
                ResponseBase.Message = "Data Berhasil Ditambahkan";
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message;
            }
            return Json(ResponseBase);
        }

        [HttpPost]
        public IActionResult Edit(long id)
        {
            VillagesService service = new(_context);
            Village? village = service.GetVillage(id);
            if (village == null)
            {
                return NotFound();
            }
            VillagesViewModel model = ModelHelper.MapProperties<Village, VillagesViewModel>(village);
            model.Districts = service.GetDistricts();
            return PartialView("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VillagesViewModel input)
        {
            try
            {
                VillagesService service = new(_context);
                VillagesRequest requestValidator = new(input, service);
                if (service.GetVillage(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Village village = ModelHelper.MapProperties<VillagesViewModel, Village>(input);
                service.Update(village, UserId);
                ResponseBase.Message = "Data Berhasil Diperbarui";
                ResponseBase.Status = StatusEnum.Success;
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message;
            }
            return Json(ResponseBase);
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            try
            {
                VillagesService service = new(_context);
                if (service.GetVillage(id) == null)
                {
                    return NotFound();
                }
                service.Delete(id, UserId);
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
