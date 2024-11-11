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
    /// API các dòng họ việt nam
    /// </summary>
    [CustomAuthorization]
    public class HoVietNamController : BaseController
    {
        private Ho_VietNamHelper _helper;
        public Ho_VietNamHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new Ho_VietNamHelper(this.user?.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Lấy danh sách  
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Danhmuc.Ho_VietNam>>))]
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            return Request.SuccessResult(helper.Find().SortBy(p => p.Name).ToList());
        }
        /// <summary>
        /// Lấy thông tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<MongoDBAccess.Models.Danhmuc.Ho_VietNam>))]
        public HttpResponseMessage Get(uint id)
        {
            var find = helper.FindById(id);
            if (find == null)
                throw new System.Exception("Không tìm thấy thông tin");
            return Request.SuccessResult(find);
        }
        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<string>))]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] MongoDBAccess.Models.Danhmuc.Ho_VietNam value)
        {
            value.Validate();
            var vCheck = helper.Find(p => p.Name == value.Name).FirstOrDefault();
            if (vCheck != null)
                throw new System.Exception("Họ này đã có trong hệ thống");
            return Request.SuccessResult(helper.Create(value, null));
        }
        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [HttpPut]
        public HttpResponseMessage Put(uint id, [FromBody] MongoDBAccess.Models.Danhmuc.Ho_VietNam value)
        {
            value.Validate();
            var vCheck = helper.Find(p => p.Name == value.Name && p.Id != value.Id).FirstOrDefault();
            if (vCheck != null)
                throw new System.Exception("Họ này đã có trong hệ thống");
            var vDongho = new DonghoHelper(this.user.Id).Find(p => p.Ho_Vietnam_id == value.Id).FirstOrDefault();
            if (vDongho != null)
                throw new System.Exception("Họ này đã có dữ liệu, bạn không thể sửa thông tin");
            return Request.SuccessResult(helper.Update(id, value.DefaultUpdateDefine(), null, null));
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
            var vDongho = new DonghoHelper(this.user.Id).Find(p => p.Ho_Vietnam_id == id).FirstOrDefault();
            if (vDongho != null)
                throw new System.Exception("Họ này đã có dữ liệu, bạn không thể xóa thông tin");
            return Request.SuccessResult(helper.Delete(id));
        }

    }
}