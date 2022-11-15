using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GroupC.Uni.Student.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GroupC.Uni.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace GroupC.Uni.Student.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _appDbContext;  
        private IConfiguration _config;
        private readonly IStudentService _studentService;
      //private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(IConfiguration config, IStudentService studentService, SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager, AppDbContext appDbContext, IOptions<JWTSettings> jwtSettings)
        {
            _config = config;
            _studentService = studentService;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _appDbContext = appDbContext;
        }
        [HttpPost]
public async Task<ActionResult<StudentModel>> Login([FromBody] LoginViewModel loginViewModel)
        {  
  
           var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                    loginViewModel.RememberMe, false);
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;

            StudentModel student = new StudentModel();
            student.Email = user.Email;
            student.guid = user.Id;
            student.UserName = user.UserName;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            student.Token = tokenHandler.WriteToken(token);
            return student;
        

            //StudentModel login = new StudentModel();
            //login.UserName = username;
            //login.Password = pass;
            //IActionResult response = Unauthorized();

            //var user = AuthenticateUser(login);
            //if (user != null)
            //{
            //    var tokenStr = GenerateJSONWebToken(user);
            //    response = Ok(new { token = tokenStr });
            //}

            //return response;
        }

        //public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
        //            loginViewModel.RememberMe, false);
        //        if (result.Succeeded)
        //        {
        //           return result
        //        }
        //        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        //    }
        //}

        private StudentModel AuthenticateUser([FromBody] StudentModel student)
        {
             StudentModel user = null;
            //    //if()

            //    //var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;

            //    //User
            //    //if (login.UserName == "rami" && login.Password == "123")
            //    //{
            //    //    user = new StudentModel { UserName = "rami", Password = "123" };
            //    //}
           return user;

            //    //var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            //    //var userd= _signInManager.
            //    //var user = _studentService.GetStudentById(x => x.Username == username && x.Password == password);

            //    //// return null if user not found
            //    //if (user == null)
            //    //    return null;

            //    //// authentication successful so generate jwt token
            //    //var tokenHandler = new JwtSecurityTokenHandler();
            //    //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //    //var tokenDescriptor = new SecurityTokenDescriptor
            //    //{
            //    //    Subject = new ClaimsIdentity(new Claim[]
            //    //    {
            //    //        new Claim(ClaimTypes.Name, user.Id.ToString())
            //    //    }),
            //    //    Expires = DateTime.UtcNow.AddDays(7),
            //    //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //    //};
            //    //var token = tokenHandler.CreateToken(tokenDescriptor);
            //    //user.Token = tokenHandler.WriteToken(token);

            //    //// remove password before returning
            //    //user.Password = null;

            //    //return user;
            }

            private string GenerateJSONWebToken(StudentModel userInfo)
        {
            // sec key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            //signing token
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userInfo.UserName),
                //new Claim(JwtRegisteredClaimNames.Email,userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //create token 
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims);



            //return token
            //write token will return string
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }
        [HttpPost("Post")]
        //[Authorize]
        public string Post()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            {
                return "authoriation header was not found";
            }
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome to: " + userName;
        }
        [HttpGet("GetValue")]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Value1", "Value2" };
        }
    }
}