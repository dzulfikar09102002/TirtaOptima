using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Controllers
{
    [Authorize(Roles = "Administrator Sistem")]
    public class LeadersController : BaseController
    {
        public IActionResult Index()
        {
            LeaderService service = new(_context);
            LeaderViewModel model = new LeaderViewModel
            {
                Leaders = service.GetLeaders()
            };
            return View(model);
        }

        public IActionResult SetLeader()
        {
            UserService service = new(_context);
            UserViewModel model = new UserViewModel
            {
                Users = service.GetUsers().Where(x => x.RoleId != 4).ToList(),
            };
            return View("SetLeader", model);
        }

        [HttpPost]
        public IActionResult SetLeader(long id)
        {
            try
            {
                UserService service = new(_context);
                User? checkuser = service.GetUser(id);
                if (checkuser == null)
                {
                    return NotFound();
                }
                service.SetLeader(id, UserId);
                ResponseBase.Message = "Berhasil menjadi pimpinan";
                ResponseBase.Status = StatusEnum.Success;
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
            LeaderService service = new(_context);
            Leader? user = service.GetLeader(id);
            if (user == null)
            {
                return NotFound();
            }
            LeaderViewModel model = ModelHelper.MapProperties<Leader, LeaderViewModel>(user);
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Update(LeaderViewModel input, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                LeaderService service = new(_context);
                LeaderRequest requestValidator = new(input, service);
                Leader? checkleader = service.GetLeader(input.Id);
                if (checkleader == null)
                {
                    throw new Exception("Pimpinan tidak ditemukan");
                }
                if (!requestValidator.Validate())
                {
                    ResponseBase.Message = requestValidator.ErrorMessage ?? "Terjadi Kesalahan";
                    throw new Exception();
                }
                if (input.Img != null && input.Img.Length > 0)
                {
                    var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    if (input.Img != null)
                    {
                        var fileName = $"{input.Id}-{input.UserId}-{timeStamp}{Path.GetExtension(input.Img?.FileName)}";
                        var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "assets/images/signatures");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        if (!string.IsNullOrEmpty(checkleader.Signature))
                        {
                            var oldName = "assets/images/signatures/" + checkleader.Signature.TrimStart('/');
                            var oldFilePath = Path.Combine(webHostEnvironment.WebRootPath, oldName);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            input.Img?.CopyTo(stream);
                        }
                        input.Signature = $"{fileName}";
                    }
                }
                else
                {
                    input.Signature = checkleader.Signature;
                }
                service.Update(input, UserId);
                ResponseBase.Status = StatusEnum.Success;
                ResponseBase.Message = "Data Berhasil Ditambahkan";
            }
            catch (Exception ex)
            {
                ResponseBase.Message = ex.Message;
            }
            return Json(ResponseBase);
        }

    }
}
