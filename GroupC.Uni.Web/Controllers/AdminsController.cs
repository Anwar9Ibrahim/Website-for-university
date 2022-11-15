using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Web.Models;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GroupC.Uni.Web.Controllers
{
    [Authorize(Roles = "AdminsManagement")]
    public class AdminsController : BaseController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IAdminService _adminService;
        //remember u didnit add the service in the startup 
        private readonly IHostingEnvironment _hostingEnv;
        public AdminsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<Guid>> roleManager, IAdminService adminService, IHostingEnvironment hostingEnv) : base(hostingEnv)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _adminService = adminService;
            _hostingEnv = hostingEnv;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }
            ViewBag.CurrentPage = "ListAdmin";
            var AdminsList = await _adminService.ListAllAdmins();
            var CreateUserViewModelList = new List<ProfileViewModel>();
            foreach (var admin in AdminsList)
            {
                ProfileViewModel currAdmin = new ProfileViewModel()
                {
                    Id = admin.Id,
                    Name = admin.ApplicationUser.Name,
                    Email = admin.ApplicationUser.Email,
                    Phone = admin.ApplicationUser.PhoneNumber,
                    UserType = admin.userType,
                    ImageURL = admin.ApplicationUser.ImageURL,
                };
                CreateUserViewModelList.Add(currAdmin);
            }
            return View(CreateUserViewModelList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CurrentPage = "CreateAdmin";
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "AdminsManagement")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel CreateUserViewModel)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = GetUniqueFileName(CreateUserViewModel);
                string dateTime = System.DateTime.Now.ToString();
                ////////////////////////////////////
                var user = new ApplicationUser
                {
                    UserName = CreateUserViewModel.Email,
                    Name = CreateUserViewModel.Name,
                    Email = CreateUserViewModel.Email,
                    PhoneNumber = CreateUserViewModel.Phone,
                    UserType = MyEnums.UserType.Admin,
                    ImageURL = uniqueFileName,
                    CreationDate = dateTime,
                    EmailConfirmed = true
                };
                Admin admin = new Admin();

                user.Admin = admin;

                var result = await _userManager.CreateAsync(user, CreateUserViewModel.Password);
                if (result.Succeeded)
                {
                    var roles = _roleManager.Roles;
                   // var userWithRoles = await _userManager.FindByEmailAsync(CreateUserViewModel.Email);

                    IdentityResult resultUserWithRole = null;

                    foreach (var role in roles)
                    {
                        resultUserWithRole = await _userManager.AddToRoleAsync(user, role.Name);
                    }

                    if (resultUserWithRole.Succeeded)
                    {
                        //AddSuccessMessage("Operation Done Successfully");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(CreateUserViewModel);
        }
        //[HttpGet]
        //public IActionResult ChangePassword()
        //{
        //    return View();
        //}
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await _userManager.ChangePasswordAsync(user,
                    model.changePasswordViewModel.CurrentPassword, model.changePasswordViewModel.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await _signInManager.RefreshSignInAsync(user);
                //password changed successfully errroooorrrr heeerrreee
                //return View("Index");
                return RedirectToAction("Index");
            }

            return View(model);
        }
        /* public string GetUniqueFileName(CreateUserViewModel CreateUserViewModel)
         {
             string uniqueFileName = null;
             if (CreateUserViewModel.Image != null)
             {
                 string uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "images");
                 string imageId = Guid.NewGuid().ToString();
                 uniqueFileName = imageId + "_" + CreateUserViewModel.Image.FileName;
                 string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                 CreateUserViewModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                 uniqueFileName = "/images/" + imageId + "_" + CreateUserViewModel.Image.FileName;
             }
             else
             {
                 uniqueFileName = "/images/userDefaultImage.png";
             }
             return uniqueFileName;
         }*/
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
        //        return View("Index");
        //    }
        //    else
        //    {
        //        if (user.UserType != MyEnums.UserType.Admin)
        //        {
        //            var result = await _userManager.DeleteAsync(user);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index");
        //            }
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //            return View("index");
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMessage = $"User with Id = {id} is an admin";
        //            return View("Index");
        //        }
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            var admin = await _adminService.GetAdminById(user.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {user.Id} cannot be found";
                //return error message
                return View("Index");
            }
            ProfileViewModel ProfileViewModel = new ProfileViewModel
            {
                Id = admin.Id,
                Email = admin.ApplicationUser.Email,
                Name = admin.ApplicationUser.Name,
                ImageURL = admin.ApplicationUser.ImageURL,
                Phone = admin.ApplicationUser.PhoneNumber,
                UserType = admin.ApplicationUser.UserType
            };
            return View(ProfileViewModel);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }
            Guid gId = Guid.Parse(id);
            var user = await _adminService.GetAdminById(gId);
            if (user == null)
            {
                //Ask Anwar
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                return View("Index");
            }

            ProfileViewModel createUserViewModel = new ProfileViewModel
            {
                Id = user.Id,
                Email = user.ApplicationUser.Email,
                Name = user.ApplicationUser.Name,
                ImageURL = user.ApplicationUser.ImageURL,
                Phone = user.ApplicationUser.PhoneNumber,
                UserType = user.ApplicationUser.UserType
            };
            return View(createUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel createUserViewModel)
        {
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            var admin = await _adminService.GetAdminById(user.Id);
            user.Admin = admin;
            if (user == null)
            {
                //Ask Anwar
                ViewBag.ErrorMessage = $"User with Id = {createUserViewModel.Id} cannot be found";
                TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                return View("Index");
            }
            else
            {
                string uniqueFileName = GetuniqueNameEdited(createUserViewModel);
                user.LastUpdateDate = System.DateTime.Now.ToString();
                user.ImageURL = uniqueFileName;
                user.Name = createUserViewModel.Name;
                user.PhoneNumber = createUserViewModel.Phone;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage("Admin: " + createUserViewModel.Name + " was Edited Successfully!"));
                  return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                return View(createUserViewModel.Id);
            }
        }
        //public async Task<IActionResult> Dance(string id)
        //{
        //    return View();
        //}
    }
}