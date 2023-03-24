using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web_Scrapping.Models;

namespace Web_Scrapping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _config;

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin )
        {
            var user = AuthenticateUser(userLogin);

            if (user != null)
            {
                var token = GenerateToken(user);

                return Ok(token);
            }

            return NotFound("user details is empty");
        }

        public string GenerateToken(UserInfo user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.Role, user.Role),
                }),

                Expires = DateTime.Now.AddMinutes(30),
                Issuer = _config["jwt:Issuer"],
                Audience = _config["jwt:Audience"],
                SigningCredentials = credentials,





            };



            var tokenhandler = new JwtSecurityTokenHandler();

            var stoken = tokenhandler.CreateToken(tokenDescriptor);

            var token = tokenhandler.WriteToken(stoken);

            return token;

            // second method

            //var claims = new[]
            //{
            //   new  Claim(ClaimTypes.NameIdentifier, user.Username),
            //   new  Claim(ClaimTypes.Email, user.EmailAddress),
            //   new  Claim(ClaimTypes.Role, user.Role),
            //};

            //var token = new JwtSecurityToken(_config["jwt:Issuer"],
            //    _config["jwt:Audience"],
            //    claims:claims,
            //    expires: DateTime.Now.AddMinutes(30),
            //    signingCredentials: credentials
            //    );

            //var tokenhandler = new JwtSecurityTokenHandler();

            //var stoken = tokenhandler.WriteToken(token);

            //return stoken;


        }

        private UserInfo AuthenticateUser(UserLogin userLogin)
        {
            var currentUser = UserValues.users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() &&
             o.Password.ToLower() == userLogin.Password.ToLower());

            if(currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}
