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
    public class ActionTypesController : BaseController
    {
        public IActionResult Index()
        {
            ActionTypeService service = new(_context);
            ActionTypeViewModel model = new ActionTypeViewModel
            {
                Actions = service.GetActions()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new ActionTypeViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ActionTypeViewModel input)
        {
            try
            {
                ActionTypeService service = new(_context);
                ActionTypeRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                ActionType action = ModelHelper.MapProperties<ActionTypeViewModel, ActionType>(input);
                service.Store(action, UserId);
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
            ActionTypeService service = new(_context);
            ActionType? action = service.GetAction(id);
            if (action == null)
            {
                return NotFound();
            }
			ActionTypeViewModel model = ModelHelper.MapProperties<ActionType, ActionTypeViewModel>(action); 
            return PartialView("Edit", model);
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(ActionTypeViewModel input)
        {
            try
            {
                ActionTypeService service = new(_context);
                ActionTypeRequest requestValidator = new(input, service);
                if (service.GetAction(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                ActionType action = ModelHelper.MapProperties<ActionTypeViewModel, ActionType>(input);
                service.Update(action, UserId);
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
                ActionTypeService service = new(_context);
                if (service.GetAction(id) == null)
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
