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
    public class RolesController : BaseController
    {
        public IActionResult Index()
        {
            RoleService service = new(_context);
            RoleViewModel model = new RoleViewModel
            {
                Roles = service.GetRoles()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new RoleViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(RoleViewModel input)
        {
            try
            {
                RoleService service = new(_context);
                RoleRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Role role = ModelHelper.MapProperties<RoleViewModel, Role>(input);
                service.Store(role, UserId);
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
            RoleService service = new(_context);
            Role? role = service.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }
            RoleViewModel model = ModelHelper.MapProperties<Role, RoleViewModel>(role);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(RoleViewModel input)
        {
            try
            {
                RoleService service = new(_context);
                RoleRequest requestValidator = new(input, service);
                if (service.GetRole(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Role role = ModelHelper.MapProperties<RoleViewModel, Role>(input);
                service.Update(role, UserId);
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
                RoleService service = new(_context);
                if (service.GetRole(id) == null)
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
