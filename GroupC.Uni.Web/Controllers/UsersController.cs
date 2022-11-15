//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using GroupC.Uni.Core.Entities;
//using GroupC.Uni.Core.Interfaces;
//using GroupC.Uni.Web.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace GroupC.Uni.Web.Controllers
//{
//    //[Authorize(Roles = "UsersManagement")]
//    public class UsersController : BaseController
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly Core.Interfaces.IStudentService _studentService;
//        private readonly ITestCenterService _testCenterService;
//        //private readonly IHostingEnvironment _hostingEnv;
//        private readonly RoleManager<IdentityRole> _roleManager;


//        public UsersController(UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager, Core.Interfaces.IStudentService studentService,
//            ITestCenterService testCenterService, IHostingEnvironment hostingEnv, RoleManager<IdentityRole> roleManager    ): base(hostingEnv)
//        {
//              _roleManager = roleManager;
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _studentService = studentService;
//            _testCenterService = testCenterService;
//           // _hostingEnv = hostingEnv;
//        }

//        //public UserManager<IdentityUser> UserManager { get; }
//        //public SignInManager<IdentityUser> SignInManager { get; }


//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Index()
//        {
//            var users = _userManager.Users;
//            return View(users);
//        }
//        [HttpGet]
//        public IActionResult CreateRole()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                // We just need to specify a unique role name to create a new role
//                IdentityRole identityRole = new IdentityRole
//                {
//                    Name = model.RoleName
//                };

//                // Saves the role in the underlying AspNetRoles table
//                IdentityResult result = await _roleManager.CreateAsync(identityRole);

//                if (result.Succeeded)
//                {
//                    return RedirectToAction("index", "home");
//                }

//                foreach (IdentityError error in result.Errors)
//                {
//                    ModelState.AddModelError("", error.Description);
//                }
//            }

//            return View(model);
//        }
//        // GET: Users



//        //////////////
//        //[HttpGet]
//        //public IActionResult CreateAdmin()
//        //{
//        //    ViewBag.CurrentPage = "CreateAdmin";
//        //    return View();
//        //}

//        //// POST: Questions/Create
//        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> CreateAdmin(CreateUserViewModel CreateUserViewModel)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        string uniqueFileName = GetUniqueFileName(CreateUserViewModel);
//        //        string dateTime = System.DateTime.Now.ToString();
//        //        ////////////////////////////////////
//        //        var user = new ApplicationUser
//        //        {
//        //            UserName = CreateUserViewModel.Email,
//        //            Name = CreateUserViewModel.Name,
//        //            Email = CreateUserViewModel.Email,
//        //            PhoneNumber = CreateUserViewModel.Phone,
//        //            UserType = MyEnums.UserType.Admin,
//        //            ImageURL = uniqueFileName,
//        //            CreationDate = dateTime,
//        //            EmailConfirmed = true
//        //        };
//        //        Admin admin = new Admin();

//        //        user.Admin = admin;

//        //        var result = await _userManager.CreateAsync(user, CreateUserViewModel.Password);
//        //        if (result.Succeeded)
//        //        {
//        //            //AddSuccessMessage("Operation Done Successfully");
//        //            //return RedirectToAction("ListTestCenters");
//        //            return View();
//        //        }
//        //        foreach (var error in result.Errors)
//        //        {
//        //            ModelState.AddModelError("", error.Description);
//        //        }
//        //    }
//        //    return View(CreateUserViewModel);
//        //}
//        ////    public async Task<IActionResult> ListTestCenters()
//        //    {
//        //        ViewBag.CurrentPage = "ViewTestCenter";
//        //        var TestCenterList = await _testCenterService.ListAllTestCenters();
//        //        var CreateUserViewModelList = new List<CreateUserViewModel>();
//        //        foreach (var testCenter in TestCenterList)
//        //        {
//        //            CreateUserViewModel currTcenter = new CreateUserViewModel()
//        //            {
//        //                Name = testCenter.ApplicationUser.Name,
//        //                Email = testCenter.ApplicationUser.Email,
//        //                Phone = testCenter.ApplicationUser.PhoneNumber,
//        //                UserType = testCenter.userType,
//        //                //Year = testCenter.Year
//        //            };
//        //            CreateUserViewModelList.Add(currTcenter);
//        //        }
//        //        return View(CreateUserViewModelList);
//        //    }
//        //}
//        //public string GetUniqueFileName(CreateUserViewModel CreateUserViewModel)
//        //{
//        //    string uniqueFileName = null;
//        //    if (CreateUserViewModel.Image != null)
//        //    {
//        //        string uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "images");
//        //        string imageId = Guid.NewGuid().ToString();
//        //        uniqueFileName = imageId + "_" + CreateUserViewModel.Image.FileName;
//        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
//        //        CreateUserViewModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
//        //        uniqueFileName = "/images/" + imageId + "_" + CreateUserViewModel.Image.FileName;
//        //    }
//        //    else
//        //    {
//        //        uniqueFileName = "/images/userDefaultImage.png";
//        //    }
//        //    return uniqueFileName;
//        //}
//        //public async Task<IActionResult> Delete(string id)
//        //{
//        //    var user = await _userManager.FindByIdAsync(id);
//        //    if (user == null)
//        //    {
//        //        ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
//        //        return View("Index");
//        //    }
//        //    else
//        //    {
//        //        if (user.UserType != MyEnums.UserType.Admin)
//        //        {
//        //            var result = await _userManager.DeleteAsync(user);
//        //            if (result.Succeeded)
//        //            {
//        //                return RedirectToAction("Index");
//        //            }
//        //            foreach (var error in result.Errors)
//        //            {
//        //                ModelState.AddModelError("", error.Description);
//        //            }
//        //            return View("index");
//        //        }
//        //        else
//        //        {
//        //            ViewBag.ErrorMessage = $"User with Id = {id} is an admin";
//        //            return View("Index");
//        //        }
//        //    }
//        //}
//        ////[HttpGet]
//        //public async Task<IActionResult> Edit(string id)
//        //{
//        //    Guid gId = Guid.Parse(id);
//        //    var user = await _studentService.GetStudentById(gId);
//        //    if (user == null)
//        //    {
//        //        ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
//        //        //return error message
//        //        return View("Index");
//        //    }
//        //    CreateUserViewModel createUserViewModel = new CreateUserViewModel
//        //    {
//        //        Id = user.Id,
//        //        Email = user.ApplicationUser.Email,
//        //        Name = user.ApplicationUser.Name,
//        //        ImageURL = user.ApplicationUser.ImageURL,
//        //        Phone = user.ApplicationUser.PhoneNumber,
//        //        UserType = user.ApplicationUser.UserType,
//        //        Year = user.Year
//        //    };
//        //    return View(createUserViewModel);
//        //}
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Edit(CreateUserViewModel createUserViewModel)
//        //{
//        //    var user = await _userManager.FindByIdAsync(createUserViewModel.Id);
//        //    if (ModelState.IsValid)
//        //    {
//        //        string dateTime = System.DateTime.Now.ToString();
//        //        string uniqueFileName = GetUniqueFileName(createUserViewModel);
//        //        ApplicationUser student = new ApplicationUser
//        //        {
//        //            Id = createUserViewModel.Id,
//        //            Email = createUserViewModel.Email,
//        //            Name = createUserViewModel.Name,
//        //            PhoneNumber = createUserViewModel.Phone,
//        //            UserType = createUserViewModel.UserType,
//        //            LastUpdateDate = dateTime,
//        //            Student = new Student(),
//        //            ImageURL = uniqueFileName
//        //        };
//        //    }

//        //}
//    }
//}
