using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_Scrapping.Models;
using Web_Scrapping.Controllers; 


namespace Web_Scrapping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        

        private IConfiguration _config;

        private  IHttpClientFactory _IHttpClientFactory;

        private string _myurl = "https://www.football.london/";




        public UserController(IConfiguration config, IHttpClientFactory IHttpClientFactory)
        {
            _config = config;
            _IHttpClientFactory = IHttpClientFactory;
            
        }

        [HttpGet("public")]
        public IActionResult publice() 
        {
            return Ok("you are on public property");

        }
        [HttpGet("Admins")]

        //[Authorize(Roles = "adminstrator")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.Username} , you are an {currentUser.Role}");
        }

       

        [HttpGet("Users")]

        [Authorize(Roles = "user")]
        public async Task<IActionResult>  UsersEndpoint()
        {
            
            var currentUser = GetCurrentUser();

            var httpClient = _IHttpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(_myurl);

            var content = await response.Content.ReadAsStringAsync();

            var web = new HtmlWeb();
            var doc = web.Load(_myurl);
            //doc.LoadHtml(content);


            //var links = doc.DocumentNode.Descendants("lin").Select(a => a.GetAttributeValue("href", null)).Where(href => href.StartsWith("https") && href != null).ToList();

            var links = doc.DocumentNode.Descendants("a").Select(a => a.GetAttributeValue("href", null)).ToList();

            if (links != null)
            {
               var chelseaNews= links.Where(a=> a.Contains("chelsea-fc/news")).ToList();


                //return new RedirectToActionResult("AdminsEndpoint","user",chelseaNews[0]);

                var cresponse = chelseaNews[0];

                return Redirect(cresponse);

                
                

            }

            return Ok();

            

        }

        public UserInfo GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserInfo
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value

                };

              
            }

            return null;
        }

        

        

        
        

       

    }
}
