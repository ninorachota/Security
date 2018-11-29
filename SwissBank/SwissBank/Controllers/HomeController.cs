using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwissBank.Data.Models;

namespace SwissBank.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly FileReader _fileReader;

        public HomeController()
        {
            _fileReader = new FileReader();
        }

        // GET: api/Views
        [HttpGet]
        [Produces("text/html")]
        public ContentResult Get()
        {
            return _fileReader.ReadHtml("/wwwroot/index.html");
        }

        // GET: api/Views/5
        [HttpGet("{id}", Name = "Home")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Views
        [HttpPost]
        public void Post(string identity, string password)
        {
            string k =  identity + password;
        }

        // PUT: api/Views/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
