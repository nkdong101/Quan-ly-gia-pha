using API.Models;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    /// <summary>
    /// Class api controller base
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseController : ApiController
    {
        /// <summary>
        /// Token sau khi người dùng đăng nhập
        /// </summary>
        public string Token { get; set; }
        public User user { get; set; }

        public BaseController()
        {

        }

        /// <summary>
        /// closs all connection
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }

    }
}