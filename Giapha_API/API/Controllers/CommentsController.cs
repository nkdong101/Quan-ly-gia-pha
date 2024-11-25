using API.Models;
using MongoDB.Driver;
using MongoDBAccess;
using MongoDBAccess.Models;
using MongoDBAccess.Models.Extend;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    /// <summary>
    /// API góp ý của người dùng
    /// </summary>
    [CustomAuthorization]
    public class CommentsController : BaseController
    {
        private CommentHelper _helper;

        public CommentHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new CommentHelper(this.user?.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Gửi góp ý
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Comments/Send")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Add([FromBody] MongoDBAccess.Models.Comments iInfo)
        {
            if (this.user != null)
                iInfo.UserId = this.user.Id;
            return Request.SuccessResult(helper.Add(iInfo));
        } 
        
        [ResponseType(typeof(APIResult<string>))]
        [Route("Comments/GetNoti")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetNoti()
        {

            return Request.SuccessResult(helper.GetNoti());
        }

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Comments/GetList")]
        [HttpGet]
        //[AllowAnonymous]
        public HttpResponseMessage GetList()
        {
            return Request.SuccessResult(helper.GetList());
        } 
        
        
    }
}