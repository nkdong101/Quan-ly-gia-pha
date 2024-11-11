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
    /// API Quản lý nhóm quyền
    /// </summary>
    [CustomAuthorization]
    public class GroupPermissionController : BaseController
    {
        private GroupPermissionHelper _helper;

        public GroupPermissionHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new GroupPermissionHelper(this.user.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Lấy danh sách nhóm quyền
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<GroupPermission>>))]
        public HttpResponseMessage Get()
        {
            return Request.SuccessResult(helper.Find().ToList());
        }

        /// <summary>
        /// Lấy thông tin của 1 ngân hàng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<GroupPermission>))]
        public HttpResponseMessage Get(uint id)
        {
            var find = helper.FindById(id);
            if (find == null)
                throw new System.Exception("Không tìm thấy thông tin");
            return Request.SuccessResult(find);
        }

        /// <summary>
        /// Thêm Ngân hàng
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Post([FromBody] GroupPermission value)
        {
            return Request.SuccessResult(helper.Create(value, null));
        }

        /// <summary>
        /// Sửa thông tin ngân hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Put(uint id, [FromBody] GroupPermission value)
        {
            return Request.SuccessResult(helper.ProcessUpdate(id, value));
        }

        /// <summary>
        /// Xóa thông tin ngân hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Delete(uint id)
        {
            return Request.SuccessResult(helper.ProcessDelete(id));
        }
    }
}