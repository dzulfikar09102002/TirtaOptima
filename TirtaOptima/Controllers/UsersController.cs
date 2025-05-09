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
    public class UsersController : BaseController
    {
        public IActionResult Index()
        {
            UserService service = new(_context);
            UserViewModel model = new UserViewModel
            {
                Users = service.GetUsers()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {
            UserService service = new(_context);
            UserViewModel model = new UserViewModel
            {
                Roles = service.GetRoles()
            };
            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UserViewModel input, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                UserService service = new(_context);
                UserRequest requestValidator = new(input, service);
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                User user = ModelHelper.MapProperties<UserViewModel, User>(input);
                if (input.Img != null && input.Img.Length > 0)
                {
                    var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    var fileName = $"{input.NikNip}-{input.Username}-{timeStamp}{Path.GetExtension(input.Img.FileName)}";
                    var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "assets/images/users");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        input.Img.CopyTo(stream);
                    }
                    user.Photo = $"{fileName}";
                }
                service.Store(user, UserId);
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
            UserService service = new(_context);
            User? user = service.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            UserViewModel model = ModelHelper.MapProperties<User, UserViewModel>(user);
            model.Roles = service.GetRoles();
            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UserViewModel input, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                UserService service = new(_context);
                UserRequest requestValidator = new(input, service);
                User? checkuser = service.GetUser(input.Id);
                if (checkuser == null)
                {
                    return NotFound();
                }
                if (!requestValidator.Validate())
                {
                    throw new Exception(requestValidator.ErrorMessage);
                }
                User user = ModelHelper.MapProperties<UserViewModel, User>(input);
                if (string.IsNullOrEmpty(input.ConfirmPassword))
                {
                    user.Password = null;
                }
                var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (input.Img != null)
                {

                    var fileName = $"{input.NikNip}-{input.Username}-{timeStamp}{Path.GetExtension(input.Img?.FileName)}";
                    var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "assets/images/users");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    if (!string.IsNullOrEmpty(checkuser.Photo))
                    {
                        var oldName = "assets/images/users/" + checkuser.Photo.TrimStart('/');
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
                    user.Photo = $"{fileName}";
                }
                service.Update(user, UserId);
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
                UserService service = new(_context);
                if (service.GetUser(id) == null)
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
