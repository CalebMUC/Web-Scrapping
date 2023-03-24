using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web_Scrapping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        private string _myurl = "https://www.football.london/";

        private IHttpClientFactory _IHttpClientFactory;

        public InformationController(IHttpClientFactory IHttpClientFactory)
        {
            _IHttpClientFactory = IHttpClientFactory;
        }

        public async Task<ActionResult<string>> GetInformation(string url)
        {
            url = _myurl;

            var httpClient =  _IHttpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            //var links = doc.DocumentNode.Descendants("lin").Select(a => a.GetAttributeValue("href", null)).Where(href => href.StartsWith("https") && href != null).ToList();

            var links = doc.DocumentNode.Descendants("Link").Select(link => link.GetAttributeValue("href", null)).ToList();
            return Ok(links);

        }

    }
}
