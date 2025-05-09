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
    public class LetterCategoriesController : BaseController
    {
        public IActionResult Index()
        {
            LettersCategoryService service = new(_context);
            LettersCategoryViewModel model = new LettersCategoryViewModel
            {
                LetterCategories = service.GetLetterCategories()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new LettersCategoryViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(LettersCategoryViewModel input)
        {
            try
            {
                LettersCategoryService service = new(_context);
                LettersCategoryRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                LetterCategory letterCategory = ModelHelper.MapProperties<LettersCategoryViewModel, LetterCategory>(input);
                service.Store(letterCategory, UserId);
                ResponseBase.Status = StatusEnum.Success;
                ResponseBase.Message = "Data berhasil ditambahkan";
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
            LettersCategoryService service = new(_context);
            LetterCategory? letterCategory = service.GetLetterCategory(id);
            if (letterCategory == null)
            {
                return NotFound();
            }
            LettersCategoryViewModel model = ModelHelper.MapProperties<LetterCategory, LettersCategoryViewModel>(letterCategory);
            return PartialView("Edit", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(LettersCategoryViewModel input)
        {
            try
            {
                LettersCategoryService service = new(_context);
                LettersCategoryRequest requestValidator = new(input, service);
                if (service.GetLetterCategory(input.Id) == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                LetterCategory action = ModelHelper.MapProperties<LettersCategoryViewModel, LetterCategory>(input);
                service.Update(action, UserId);
                ResponseBase.Message = "Data berhasil diperbarui";
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
                LettersCategoryService service = new(_context);
                if (service.GetLetterCategory(id) == null)
                {
                    return NotFound();
                }
                service.Delete(id, UserId);
                ResponseBase.Message = "Data berhasil dihapus";
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




