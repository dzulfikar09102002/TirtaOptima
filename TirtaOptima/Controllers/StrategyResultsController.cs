using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem, Pimpinan")]
    public class StrategyResultsController : BaseController
    {
        public IActionResult Index()
        {
            var bulan = TempData.Peek("Bulan-debts")?.ToString();
            var tahun = TempData.Peek("Tahun-debts")?.ToString();

            StrategyResultViewModel model = new();

            if (!string.IsNullOrEmpty(bulan) && !string.IsNullOrEmpty(tahun))
            {
                model.BulanSelect = Convert.ToInt32(bulan);
                model.TahunSelect = Convert.ToInt32(tahun);

                TempData.Keep("Bulan-debts");
                TempData.Keep("Tahun-debts");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GetData(string bulan, string tahun)
        {
            StrategyResultService service = new(_context);
            StrategyResultViewModel model = new()
            {
                BulanSelect = Convert.ToInt32(bulan),
                TahunSelect = Convert.ToInt32(tahun)
            };
            StrategyResultRequest requestValidator = new(model, service);
            try
            {
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? throw new Exception();
                    return Json(ResponseBase.Message);
                }
                model.Results = service.GetScore(model, UserId);
                return PartialView("GetData", model);
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message ?? throw new Exception();
                return Json(ResponseBase);
            }
        }
        [HttpPost]
		public IActionResult AssignCollector(string[] selecteditems)
		{
			AssignCollectorService service = new(_context);
			UserService userService = new(_context);

			var selected = service.GetItems(selecteditems);

			TempData["SelectedItemsJson"] = JsonConvert.SerializeObject(selected);

			TempData.Keep("SelectedItemsJson");

			StrategyResultViewModel model = new StrategyResultViewModel
			{
				Users = userService.GetUsers().Where(x => x.Role?.Name == "Petugas Penagihan").ToList()
			};

			return PartialView(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Save(StrategyResultViewModel input)
		{
			try
			{
				AssignCollectorService service = new(_context);

				if (input.Penagih == null)
				{
					ResponseBase.Message = "Petugas Penagihan harus dipilih";
					return Json(ResponseBase);
				}

				if (TempData["SelectedItemsJson"] == null)
				{
					ResponseBase.Message = "Data belum dipilih. Silakan ulangi.";
					return Json(ResponseBase);
				}

				var selectedItems = JsonConvert.DeserializeObject<List<AssignColumn>>(TempData["SelectedItemsJson"]?.ToString() ?? "");

				if (selectedItems == null || !selectedItems.Any())
				{
					ResponseBase.Message = "Data item kosong.";
					return Json(ResponseBase);
				}

				input.SelectedItems = selectedItems;

				service.Save(input, UserId);

				ResponseBase.Message = "Berhasil melakukan penugasan";
				ResponseBase.Status = StatusEnum.Success;
			}
			catch (Exception ex)
			{
				ResponseBase.Message = ex.Message ?? throw new Exception();
			}

			return Json(ResponseBase);
		}

	}
}
