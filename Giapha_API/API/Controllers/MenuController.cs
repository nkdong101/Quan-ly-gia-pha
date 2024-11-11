using API.Models;
using MongoDB.Driver;
using MongoDBAccess;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    [CustomAuthorization]
    public class MenuController : BaseController
    {
        private MenuHelper _helper;

        public MenuHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new MenuHelper(this.user.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Lấy danh sách Menu
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MenuResult>>))]
        public HttpResponseMessage GetMenu()
        {
            return Request.SuccessResult(this.user.GetMenu(true));
        }

        /// <summary>
        /// Lấy thông tin của 1 Menu theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<Menu>))]
        public HttpResponseMessage Get(uint id)
        {
            var find = helper.FindById(id);
            if (find == null)
                throw new NotFoundExeption("Không tìm thấy thông tin");
            return Request.SuccessResult(find);
        }

        /// <summary>
        /// Thêm Menu
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Post([FromBody] Menu value)
        {
            return Request.SuccessResult(helper.Create(value,null));
        }

        /// <summary>
        /// Sửa thông tin Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Put(uint id, [FromBody] Menu value)
        {
            return Request.SuccessResult(helper.Update(id, value.DefaultUpdateDefine(), null));
        }

        /// <summary>
        /// Xóa thông tin menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Delete(uint id)
        {
            return Request.SuccessResult(helper.Delete(id));
        }
    }
}