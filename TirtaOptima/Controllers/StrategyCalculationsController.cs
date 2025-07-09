using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem, Pimpinan")]
    public class StrategyCalculationsController : BaseController
    {
        public IActionResult Index()
        {
            var service = new StrategyCalculationService(_context);

            var model = new StrategyCalculationViewModel
            {
                Criterias = service.GetCriterias(),
                FuzzyComparisons = service.GetFuzzyComparisons(),
                CriteriaNames = _context.Criterias
                    .OrderBy(c => c.Id)
                    .Select(c => c.Name ?? $"Kriteria {c.Id}")
                    .ToList(),
                NormalisasiDetails = service.GetNormalisasiDetails(),
                PsoLogs = service.GetPsoLogSummary()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Input(long kriteria1Id, long kriteria2Id)
        {
            StrategyCalculationService service = new(_context);
            StrategyCalculationViewModel model = new StrategyCalculationViewModel
            {
                CriteriaComparison = service.GetCriteriaComparison(kriteria1Id, kriteria2Id)
            };
            return PartialView("Input", model);
        }
        public IActionResult SaveScale(StrategyCalculationViewModel input)
        {
            try
            {   
                StrategyCalculationService service = new(_context);
                StrategyCalculationRequest requestValidator = new(input, service);

                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan";
                    ResponseBase.Status = StatusEnum.Error;
                    return Json(ResponseBase);
                }

                CriteriaComparison comparison = ModelHelper.MapProperties<StrategyCalculationViewModel, CriteriaComparison>(input);
                service.Save(comparison, input.IsRepricoral);

                ResponseBase.Message = "Berhasil disimpan";
                ResponseBase.Status = StatusEnum.Success;
            }
            catch (Exception ex)
            {
                ResponseBase.Message = "Terjadi kesalahan saat menyimpan data: " + ex.Message;
                ResponseBase.Status = StatusEnum.Error;
            }

            return Json(ResponseBase);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(StrategyCalculationViewModel input)
        {
            try
            {
                FuzzyAhpPsoService service = new(_context, input.Option);
                StrategyCalculationService _service = new(_context);

                var result = await service.CalculateFromDatabaseAsync();
                _service.Store(result, UserId);
                ResponseBase.Status = StatusEnum.Success;
                ResponseBase.Message = "Berhasil dihitung";
            }
            catch (Exception ex)
            {
                ResponseBase.Message = "Terjadi kesalahan saat menyimpan data: " + ex.Message;
                ResponseBase.Status = StatusEnum.Error;
            }

            return Json(ResponseBase);
        }

    }
}
