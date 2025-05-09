using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem")]
    public class DistrictsController : BaseController
    {
        public IActionResult Index()
        {
            DistrictsService service = new(_context);
            DistrictsViewModel model = new DistrictsViewModel
            {
                Districts = service.GetDistricts()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new DistrictsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(DistrictsViewModel input)
        {
            try
            {
                DistrictsService service = new(_context);
                DistrictsRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                District district = ModelHelper.MapProperties<DistrictsViewModel, District>(input);
                service.Store(district, UserId);
                ResponseBase.Status = StatusEnum.Success;
                ResponseBase.Message = "Data Berhasil Ditambahkan";
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message;
            }
            return Json(ResponseBase);
        }

        public IActionResult Detail(long id)
        {
            DistrictsService service = new(_context);
            District? district = service.GetDistrict(id);
            if (district == null)
            {
                return NotFound();
            }
            DistrictsViewModel model = new DistrictsViewModel
            {
                Villages = district.Villages.ToList()
            };
            return View("Detail", model);
        }

        [HttpPost]
        public IActionResult Edit(long id)
        {
            DistrictsService service = new(_context);
            District? district = service.GetDistrict(id);
            if (district == null)
            {
                return NotFound();
            }
            DistrictsViewModel model = ModelHelper.MapProperties<District, DistrictsViewModel>(district);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DistrictsViewModel input)
        {
            try
            {
                DistrictsService service = new(_context);
                DistrictsRequest requestValidator = new(input, service);
                if (service.GetDistrict(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                District district = ModelHelper.MapProperties<DistrictsViewModel, District>(input);
                service.Update(district, UserId);
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
                DistrictsService service = new(_context);
                if (service.GetDistrict(id) == null)
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
