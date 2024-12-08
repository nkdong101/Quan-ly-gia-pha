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
    /// API các quản lý gia phả
    /// </summary>
    [CustomAuthorization]
    public class GiaphaController : BaseController
    {
        private Gia_phaHelper _helper;

        public Gia_phaHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new Gia_phaHelper(this.user?.Id);
                return _helper;
            }
        }

        ///<summary>
        ///Thêm tiểu sử
        /// </summary>
        [Route("Giapha/AddTieuSu")]
        [ResponseType(typeof(APIResult<string>))]

        [HttpPost]
        public HttpResponseMessage AddTieuSu([FromBody] MongoDBAccess.Models.Gia_pha person)
        {
            return Request.SuccessResult(helper.AddTieuSu(person));
        }

        
        
        ///<summary>
        ///Lấy thông báo ngày giỗ 7 ngày tới
        /// </summary>
        [Route("Giapha/GetNotify")]
        [HttpGet]
        public HttpResponseMessage GetNotify()
        {
            return Request.SuccessResult(helper.ThongBaoNgayGio());
        }

        /// <summary>
        /// Lấy danh sách gia phả hiển thị dạng tree\
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Extend.Giapha_Tree>>))]
        [Route("Giapha/GetTree")]
        [HttpGet]
        public HttpResponseMessage GetTree()
        {
            return Request.SuccessResult(helper.GetTree());
        }
        /// <summary>
        /// Lấy thông tin gia đình của 1 người
        /// </summary>
        /// <param name="iPerson_id">ID người cần lấy thông tin</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<MongoDBAccess.Models.Extend.Family>))]
        [Route("Giapha/GetFamily")]
        [HttpGet]
        public HttpResponseMessage Family(uint iPerson_id)
        {
            return Request.SuccessResult(helper.GetFamily(iPerson_id));
        }
        /// <summary>
        /// Lấy thông tin của 1 thành viên trong gia phả
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<MongoDBAccess.Models.Gia_pha>))]
        public HttpResponseMessage Get(uint id)
        {
            var find = helper.FindById(id);
            if (find == null)
                throw new System.Exception("Không tìm thấy thông tin");
            if(find.Date_of_death != null)
            {
                find.Date_of_death = new Gia_phaHelper(this.user.Id).DoilichAM_DUONG(find.Date_of_death.Value);
            }
            return Request.SuccessResult(find);
        }
        /// <summary>
        /// Thêm mới bố mẹ của 1 thành viên
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Giapha/Save_Bome")]
        [HttpPost]
        public HttpResponseMessage Save_Bome([FromBody] MongoDBAccess.API_Input.Add_bome_Input value)
        {
            if (value.Bo_Info == null)
                throw new System.Exception("Thông tin của bố không được để trống!");
            if (value.Me_Info == null)
                throw new System.Exception("Thông tin của mẹ không được để trống!");
            return Request.SuccessResult(helper.Save_Bome(value));
        }
        /// <summary>
        /// Thêm mới anh chị em (chỉ thêm anh chị em ruột)
        /// Nếu anh chị em là con riêng của mẹ thì sẽ từ mẹ để thêm
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Giapha/Add_ACE")]
        [HttpPost]
        public HttpResponseMessage Add_ACE([FromBody] MongoDBAccess.API_Input.Add_ACE_Input value)
        {
            return Request.SuccessResult(helper.Add_Anh_Chi_Em(value));
        }
        /// <summary>
        /// Thêm mới con
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Giapha/Add_Con")]
        [HttpPost]
        public HttpResponseMessage Add_Con([FromBody] MongoDBAccess.API_Input.Add_Con_Input value)
        {
            return Request.SuccessResult(helper.Add_Con(value));
        }
        /// <summary>
        /// Thêm mới vợ (từ người nam thêm vợ của mình)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Giapha/Add_Vo")]
        [HttpPost]
        public HttpResponseMessage Add_Vo([FromBody] MongoDBAccess.API_Input.Add_Vo_Input value)
        {
            return Request.SuccessResult(helper.Add_Vo(value));
        }
        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [HttpPut]
        public HttpResponseMessage Put(uint id, [FromBody] MongoDBAccess.Models.Gia_pha value)
        {
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
            return Request.SuccessResult(helper.Remove(id));
        }
        /// <summary>
        /// Sắp xếp lại các đời của dòng họ
        /// </summary>
        /// <param name="iDongho_id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [HttpGet]
        [Route("Giapha/RecheckLevel")]
        public HttpResponseMessage RecheckLevel(int iDongho_id)
        {
            return Request.SuccessResult(helper.CheckLevel(iDongho_id));
        }
    }
}