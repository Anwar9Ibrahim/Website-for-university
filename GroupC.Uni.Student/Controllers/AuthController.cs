using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GroupC.Uni.Student.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("token")]
        public ActionResult GetToken()
        {
            //sec key
            string securityKey = "this_is_our_super_long_security_key_for_token_validation_project_2020_03_12$smesk.in";

            //symmetric key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing token
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);


            //add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Adminstrator"));
            claims.Add(new Claim("Student", "User"));

            //create token 
            var token = new JwtSecurityToken(
                issuer: "smesk.in",
                audience: "readers",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims);



            //return token
            //write token will return string
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}