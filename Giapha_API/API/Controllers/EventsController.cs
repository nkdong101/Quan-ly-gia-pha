using API.Models;
using MongoDBAccess.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MongoDB.Driver;

namespace API.Controllers
{
    /// <summary>
    /// API quản lý thông tin sự kiện
    /// </summary>
    [CustomAuthorization]
    public class EventsController : BaseController
    {
        /// <summary>
        /// Lấy danh sách 
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Events>>))]
        [Route("Event/Get_List")]
        [HttpGet]
        public HttpResponseMessage Get_List()
        {
            var vhelper = new Event_Helper(this.user.Id);
            return Request.SuccessResult(vhelper.GetList(this.user.Dongho_id));
        }
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Event/Add")]
        [HttpPost]
        public HttpResponseMessage Add([FromBody] MongoDBAccess.Models.Events iInfo)
        {
            var vhelper = new Event_Helper(this.user.Id);
            iInfo.Dongho_id = this.user.Dongho_id;
            return Request.SuccessResult(vhelper.Add(iInfo));
        }

        /// <summary>
        /// Sửa thông tin 
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Event/Edit")]
        [HttpPost]
        public HttpResponseMessage Edit([FromBody] MongoDBAccess.Models.Events iInfo)
        {
            var vhelper = new Event_Helper(this.user.Id);
            return Request.SuccessResult(vhelper.Edit(iInfo));
        }
        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Event/Delete")]
        [HttpPost]
        public HttpResponseMessage Delete([FromBody] MongoDBAccess.Models.Events iInfo)
        {
            var vhelper = new Event_Helper(this.user.Id);
            return Request.SuccessResult(vhelper.Remove(iInfo));
        }
        /// <summary>
        /// Hủy sự kiện
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Event/Cancel")]
        [HttpPost]
        public HttpResponseMessage Cancel([FromBody] MongoDBAccess.Models.Events iInfo)
        {
            var vhelper = new Event_Helper(this.user.Id);
            iInfo.State = MongoDBAccess.Models.Enums.EventState.Cancel;
            return Request.SuccessResult(vhelper.Cancel_Pending(iInfo));
        }
        /// <summary>
        /// Tạm dừng đăng ký sự kiện
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Event/Pending")]
        [HttpPost]
        public HttpResponseMessage Pending([FromBody] MongoDBAccess.Models.Events iInfo)
        {
            var vhelper = new Event_Helper(this.user.Id);
            iInfo.State = MongoDBAccess.Models.Enums.EventState.Pending;
            return Request.SuccessResult(vhelper.Cancel_Pending(iInfo));
        }
        /// <summary>
        /// Đăng ký tham gia sự kiện
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Event/Register")]
        [HttpPost]
        public HttpResponseMessage Register([FromBody] MongoDBAccess.Models.Event_Registers iInfo)
        {
            var vhelper = new Event_Helper(this.user.Id);
            return Request.SuccessResult(vhelper.Register(iInfo, this.user));
        }
        /// <summary>
        /// Lấy danh sách người đăng ký
        /// </summary>
        /// <param name="iEvent_id">ID sự kiện</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Event_Registers>>))]
        [Route("Event/Person_Register")]
        [HttpGet]
        public HttpResponseMessage Person_Register(uint iEvent_id)
        {
            var vhelper = new Event_Registers_Helper(this.user.Id);
            var vEvent = new Event_Helper(this.user.Id).FindById(iEvent_id);
            if (vEvent == null)
                throw new Exception("Không tìm thấy thông tin sự kiện!");
            if (vEvent.Dongho_id != this.user.Dongho_id)
                throw new Exception("Bạn không thuộc dòng họ này để có thể xem thông tin!");
            return Request.SuccessResult(vhelper.Find(p => p.Event_id == iEvent_id).ToList());
        }
        /// <summary>
        /// Lấy chi tiết thu chi của sự kiện
        /// </summary>
        /// <param name="iEvent_id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Thu_Chi>>))]
        [Route("Event/Thuchi")]
        [HttpGet]
        public HttpResponseMessage Thuchi(uint iEvent_id)
        {
            var vhelper = new Thuchi_Helper(this.user.Id);
            var vEvent = new Event_Helper(this.user.Id).FindById(iEvent_id);
            if (vEvent == null)
                throw new Exception("Không tìm thấy thông tin sự kiện!");
            if (vEvent.Dongho_id != this.user.Dongho_id)
                throw new Exception("Bạn không thuộc dòng họ này để có thể xem thông tin!");
            return Request.SuccessResult(vhelper.Find(p => p.Event_id == iEvent_id).ToList());
        }
    }
}