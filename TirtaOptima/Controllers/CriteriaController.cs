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
    public class CriteriaController : BaseController
    {
        public IActionResult Index()
        {
            CriteriaService service = new(_context);
            CriteriaViewModel model = new CriteriaViewModel
            {
                Criterias = service.GetCriterias()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new CriteriaViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CriteriaViewModel input)
        {
            try
            {
                CriteriaService service = new(_context);
                CriteriaRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Criteria criteria = ModelHelper.MapProperties<CriteriaViewModel, Criteria>(input);
                service.Store(criteria, UserId);
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
            CriteriaService service = new(_context);
            Criteria? criteria = service.GetCriteria(id);
            if (criteria == null)
            {
                return NotFound();
            }
            CriteriaViewModel model = ModelHelper.MapProperties<Criteria, CriteriaViewModel>(criteria);
            return PartialView("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(CriteriaViewModel input)
        {
            try
            {
                CriteriaService service = new(_context);
                CriteriaRequest requestValidator = new(input, service);
                if (service.GetCriteria(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                Criteria criteria = ModelHelper.MapProperties<CriteriaViewModel, Criteria>(input);
                service.Update(criteria, UserId);
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
                CriteriaService service = new(_context);
                if (service.GetCriteria(id) == null)
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
