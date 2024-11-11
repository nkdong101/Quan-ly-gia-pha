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
    public class MenuButtonController : BaseController
    {
        private MenuButtonHelper _helper;

        public MenuButtonHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new MenuButtonHelper(this.user.Id);
                return _helper;
            }
        }


        /// <summary>
        /// Thêm MenuButton
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Post([FromBody] MenuButton value)
        {
            return Request.SuccessResult(helper.Create(value, null));
        }

        /// <summary>
        /// Sửa thông tin MenuButton
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Put(uint id, [FromBody] MenuButton value)
        {
            return Request.SuccessResult(helper.Update(id, value.DefaultUpdateDefine(),null));
        }

        /// <summary>
        /// Xóa thông tin MenuButton
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