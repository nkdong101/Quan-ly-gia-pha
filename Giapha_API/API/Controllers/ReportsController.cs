using API.Models;
using MongoDB.Driver;
using MongoDBAccess;
using MongoDBAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    /// <summary>
    /// API báo cáo thống kê
    /// </summary>
    [CustomAuthorization]
    public class ReportsController : BaseController
    {

        /// <summary>
        /// Gửi góp ý
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Reports/Home")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetInfo()
        {
            var vResult = new MongoDBAccess.Models.Extend.HomeInfo()
            {
                Tinh = 20,
                Giapha = new Gia_phaHelper(1).Find().CountDocuments(),
                Dongho = new DonghoHelper(1).Find().CountDocuments()
            };
            return Request.SuccessResult(vResult);
        }
    }
}