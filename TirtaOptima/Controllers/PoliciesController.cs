using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize (Roles ="Administrator Sistem, Pimpinan")]
    public class PoliciesController : BaseController
    {
        public IActionResult Index()
        {
            PoliciesService service = new(_context);
            PoliciesViewModel model = new PoliciesViewModel
            {
                Policies = service.GetPolicies()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new PoliciesViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(PoliciesViewModel input)
        {
            try
            {
                PoliciesService service = new(_context);
                PoliciesRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Policy policy = ModelHelper.MapProperties<PoliciesViewModel, Policy>(input);
                service.Store(policy, UserId);
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
            PoliciesService service = new(_context);
            Policy? policy = service.GetPolicy(id);
            if (policy == null)
            {
                return NotFound();
            }
            PoliciesViewModel model = ModelHelper.MapProperties<Policy, PoliciesViewModel>(policy);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(PoliciesViewModel input)
        {
            try
            {
                PoliciesService service = new(_context);
                PoliciesRequest requestValidator = new(input, service);
                if (service.GetPolicy(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Policy policy = ModelHelper.MapProperties<PoliciesViewModel, Policy>(input);
                service.Update(policy, UserId);
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
                PoliciesService service = new(_context);
                if (service.GetPolicy(id) == null)
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

