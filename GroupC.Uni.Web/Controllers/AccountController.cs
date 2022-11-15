using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GroupC.Uni.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public UserManager<IdentityUser> UserManager { get; }
        public SignInManager<IdentityUser> SignInManager { get; }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("login", "Account");
        }
        [AllowAnonymous]
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Log in";
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                    loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            ViewData["Title"] = "Log in";
            return View(loginViewModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgoutPasswordViewModel model)
        {
            //try
            //{
            //    using (var client = new WebClient())
            //    using (client.OpenRead("http://google.com/generate_204")) ;
            //}
            //catch
            //{
            //    return StatusCode(500);
            //}
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if  (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new { email = model.Email, token = token }, Request.Scheme);
                    
                    MailMessage Msg = new MailMessage();
                    // Sender e-mail address.
                    Msg.From = new MailAddress("webProjectGroupC@gmail.com");
                    // Recipient e-mail address.
                    Msg.To.Add(model.Email);
                    Msg.Subject = "Set Password";
                    Msg.Body = "Hi, Please follow the link to update yourpassword on WEB Project GroupC Admin : \n" + passwordResetLink;
                    // your remote SMTP server IP.
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("webProjectGroupC@gmail.com", "jlqsgummwypspeng");
                    smtp.EnableSsl = true;
                    smtp.Send(Msg);
                    // logger.log(LogLevel.Warning, passwordResetLink);
                    //ResetPasswordViewModel viewModel = new ResetPasswordViewModel();

                    //viewModel.Token = token;
                    //viewModel.Email = model.Email;
                    //return RedirectToAction("ResetPassword", new { Token = token, Email = model.Email });
                    return View("ForgotPasswordConfirmation");

                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string Token,string email)
        {
            if ( email == null|| Token == null)
            {
                ModelState.AddModelError("","Invalid password reset token");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            //ViewBag.message = "";
            if (ModelState.IsValid)
            {
                //ViewBag.message = null;
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                     {
                        //ViewBag.message = "<p> show message </p>";
                        //return PartialView("_ResetPasswordConfirmation");
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                //return PartialView("_ResetPasswordConfirmation");
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
        
         [HttpGet]

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]  
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                //we create IdentityUser because the usermanager 
                //can only deals with Identity User
                var user = new ApplicationUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    EmailConfirmed=true
                };
                 var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(registerViewModel);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
