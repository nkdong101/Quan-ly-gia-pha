using API.Models;
using MongoDB.Driver;
using MongoDBAccess.Helper;
using MongoDBAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    /// <summary>
    /// API quản lý thu chi
    /// </summary>
    [CustomAuthorization]
    public class ThuchiController : BaseController
    {
        private Thuchi_Helper _helper;
        public Thuchi_Helper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new Thuchi_Helper(this.user?.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Lấy danh sách  
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Thu_Chi>>))]
        public HttpResponseMessage Get()
        {
            return Request.SuccessResult(helper.Find().SortBy(p => p.DateActive).ToList());
        }

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<uint>))]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Thu_Chi value)
        {
            return Request.SuccessResult(helper.Add(value));
        }
        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [HttpPut]
        public HttpResponseMessage Put(uint id, [FromBody] Thu_Chi value)
        {
            return Request.SuccessResult(helper.Edit(value));
        }
        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [HttpDelete]
        public HttpResponseMessage Delete(uint id)
        {
            return Request.SuccessResult(helper.Delete(id));
        }

    }
}