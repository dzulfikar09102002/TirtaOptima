using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    public class CustomerTypesController : BaseController
    {
        [Authorize(Roles = "Administrator Sistem")]
        public IActionResult Index()
        {
            CustomerTypeService service = new(_context);
            CustomerTypeViewModel model = new CustomerTypeViewModel
            {
                CustomerTypes = service.GetCustomerTypes()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create()
        {
            return PartialView("Create", new CustomerTypeViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CustomerTypeViewModel input)
        {
            try
            {
                CustomerTypeService service = new(_context);
                CustomerTypeRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                CustomerType customerType = ModelHelper.MapProperties<CustomerTypeViewModel, CustomerType>(input);
                service.Store(customerType, UserId);
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
        public IActionResult Edit(string id)
        {
			CustomerTypeService service = new(_context);
			CustomerType? customerType = service.GetCustomerType(id);
			if (customerType == null)
			{
				return NotFound();
			}
			CustomerTypeViewModel model = ModelHelper.MapProperties<CustomerType, CustomerTypeViewModel>(customerType);
			return PartialView("Edit", model);
		}
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(CustomerTypeViewModel input)
        {
			try
			{
				CustomerTypeService service = new(_context);
				CustomerTypeRequest requestValidator = new(input, service);
				if (!requestValidator.Validate())
				{
					throw new Exception(requestValidator.ErrorMessage);
				}
				CustomerType customerType = ModelHelper.MapProperties<CustomerTypeViewModel, CustomerType>(input);
				service.Update(customerType, UserId);
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
        public IActionResult Delete(string id)
        {
			try
			{
				CustomerTypeService service = new(_context);
				if (service.GetCustomerType(id) == null)
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
