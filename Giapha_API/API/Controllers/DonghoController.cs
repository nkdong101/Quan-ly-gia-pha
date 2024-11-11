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
    /// API các dòng họ đăng ký trên hệ thống
    /// </summary>
    [CustomAuthorization]
    public class DonghoController : BaseController
    {
        private DonghoHelper _helper;

        public DonghoHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new DonghoHelper(this.user?.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Lấy danh sách  
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Dong_ho>>))]
        public HttpResponseMessage Get(uint iHoVietNam_id)
        {
            return Request.SuccessResult(helper.Find(p => p.Ho_Vietnam_id == iHoVietNam_id).SortBy(p => p.Name).ToList());
        }
    }
}