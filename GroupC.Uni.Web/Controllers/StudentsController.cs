using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Web.Models;
//using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GroupC.Uni.Web.Controllers
{
    [Authorize(Roles = "StudentsManagement")]
    public class StudentsController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStudentService _studentService;
       // private readonly IHostingEnvironment _hostingEnv;
        public StudentsController(UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager, Core.Interfaces.IStudentService studentService,
           ITestCenterService testCenterService, IHostingEnvironment hostingEnv): base(hostingEnv)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _studentService = studentService;
           // _hostingEnv = hostingEnv;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }
            //throw new Exception("Error");
            ViewBag.CurrentPage = "ViewUser";
            var StudentsList = await _studentService.ListAllStudents();
            var CreateUserViewModelList = new List<CreateUserViewModel>();
            foreach (var student in StudentsList)
            {
                CreateUserViewModel currStudent = new CreateUserViewModel()
                {
                    Id = student.Id,
                    Name = student.ApplicationUser.Name,
                    Email = student.ApplicationUser.Email,
                    Phone = student.ApplicationUser.PhoneNumber,
                    UserType = student.userType,
                    ImageURL = student.ApplicationUser.ImageURL,
                    Year = student.Year
                };
                CreateUserViewModelList.Add(currStudent);
            }
            return View(CreateUserViewModelList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CurrentPage = "CreateStudent";
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                var user = new ApplicationUser
                {
                    UserName = CreateUserViewModel.Email,
                    Name = CreateUserViewModel.Name,
                    Email = CreateUserViewModel.Email,
                    PhoneNumber = CreateUserViewModel.Phone,
                    UserType = MyEnums.UserType.Student,
                    ImageURL = uniqueFileName,
                    CreationDate = dateTime,
                    EmailConfirmed = true

                };
                Student student = new Student();
                student.Year = CreateUserViewModel.Year;
                user.Student = student;

                var result = await _userManager.CreateAsync(user, CreateUserViewModel.Password);
                if (result.Succeeded)
                {
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage("Student: " + CreateUserViewModel.Name + " was Added Successfully!"));
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed !"));
              return View(CreateUserViewModel);
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
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return NotFound();
            }
            else
            {
                if (user.UserType != MyEnums.UserType.Admin)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("index");
                }
                else
                {
                    ViewBag.ErrorMessage = $"User with Id = {id} is an admin";
                    return View("Index");
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }
            Guid gId = Guid.Parse(id);
            var user = await _studentService.GetStudentById(gId);
            if (user == null)
            {
                //Ask Anwar
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
               return NotFound();
                //TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                //return View("Index");
            }
            CreateUserViewModel createUserViewModel = new CreateUserViewModel
            {
                Id = user.Id,
                Email = user.ApplicationUser.Email,
                Name = user.ApplicationUser.Name,
                ImageURL = user.ApplicationUser.ImageURL,
                Phone = user.ApplicationUser.PhoneNumber,
                UserType = user.ApplicationUser.UserType,
                Year = user.Year
            };
            return View(createUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateUserViewModel createUserViewModel)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }
            string sId= createUserViewModel.Id.ToString();
            var user = await _userManager.FindByIdAsync(sId);
            var userWithStudent = await _studentService.GetStudentById(createUserViewModel.Id);
            user.Student = userWithStudent.ApplicationUser.Student;

            if (user == null)
            {
                //Ask Anwar
                ViewBag.ErrorMessage = $"User with Id = {createUserViewModel.Id} cannot be found";
                return NotFound();
                //TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                //return View("Index");
            }
            else
            {
                string uniqueFileName = GetuniqueNameEdited(createUserViewModel);
                user.LastUpdateDate = System.DateTime.Now.ToString();
                user.ImageURL = uniqueFileName;
                user.Name = createUserViewModel.Name;                
                user.Student.Year = createUserViewModel.Year;
                user.PhoneNumber = createUserViewModel.Phone;

                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage("Student: " + createUserViewModel.Name + " was Edited Successfully!"));
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                return View(createUserViewModel);
            }

          

        }

    }
}
