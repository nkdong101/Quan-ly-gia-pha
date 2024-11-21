using API.Models;
using MongoDB.Driver;
using MongoDBAccess;
using MongoDBAccess.Helper;
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
    public class EventsController : BaseController
    {
        private Event_Helper _helper;

        public Event_Helper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new Event_Helper(this.user.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Lấy danh sách sự kiện
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Get()
        {
            return Request.SuccessResult(helper.GetList());
        }

        /// <summary>
        /// Thêm Events
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Post([FromBody] Events value)
        {
            return Request.SuccessResult(helper.Create(value, null));
        }

        /// <summary>
        /// Sửa thông tin Events
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Put(uint id, [FromBody] Events value)
        {
            return Request.SuccessResult(helper.Update(id, value.DefaultUpdateDefine(),null));
        }

        /// <summary>
        /// Xóa thông tin Events
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