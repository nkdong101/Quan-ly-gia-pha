using API.Models;
using API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [CustomAuthorization]
    public class ValuesController : BaseController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("API/Values/Login")]
        [HttpPost]
        [AllowAnonymous]
        public string Login([FromBody] LoginModel model)
        {
            var uri = Request.RequestUri.Authority;

            var a = new LoginUlti(model.iUsername)
            {
                Authority = uri
            };
            
            return a.GetAccessToken(model.iPassword);
        }
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
