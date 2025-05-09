using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize (Roles = "Administrator Sistem")]
    public class StatusCodesController : BaseController
    {
        public IActionResult Index()
        {
            StatusCodesService service = new(_context);
            StatusCodesViewModel model = new StatusCodesViewModel
            {
                Statuses = service.GetStatuses()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new StatusCodesViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(StatusCodesViewModel input)
        {
            try
            {
                StatusCodesService service = new(_context);
                StatusCodesRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Status status = ModelHelper.MapProperties<StatusCodesViewModel, Status>(input);
                service.Store(status, UserId);
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
            StatusCodesService service = new(_context);
            Status? status = service.GetStatus(id);
            if (status == null)
            {
                return NotFound();
            }
            StatusCodesViewModel model = ModelHelper.MapProperties<Status, StatusCodesViewModel>(status);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(StatusCodesViewModel input)
        {
            try
            {
                StatusCodesService service = new(_context);
                StatusCodesRequest requestValidator = new(input, service);
                if (service.GetStatus(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Status status = ModelHelper.MapProperties<StatusCodesViewModel, Status>(input);
                service.Update(status, UserId);
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
                StatusCodesService service = new(_context);
                if (service.GetStatus(id) == null)
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
