using API.Models;
using MongoDB.Driver;
using MongoDBAccess;
using MongoDBAccess.Models;
using MongoDBAccess.Models.Extend;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Utility;

namespace API.Controllers
{
    /// <summary>
    /// API quản lý đơn vị hành chính
    /// </summary>
    [CustomAuthorization]
    public class DonviHanhchinhController : BaseController
    {
        private DonviHanhchinhHelper _helper;

        public DonviHanhchinhHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new DonviHanhchinhHelper(this.user?.Id);
                return _helper;
            }
        }
        #region Tỉnh

        /// <summary>
        /// Lấy danh sách  
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh>>))]
        [Route("DonviHanhchinh/Get_Tinh")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get_Tinh()
        {
            return Request.SuccessResult(helper.Find().SortBy(p => p.Name).ToList());
        }

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="value"></param>
        [ResponseType(typeof(APIResult<uint>))]
        [Route("DonviHanhchinh/Add_Tinh")]
        [HttpPost]
        public HttpResponseMessage Add_Tinh([FromBody] MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh value)
        {
            value.Level = 0;
            value.Parent_id = 0;
            var vCheck = helper.Find(p => p.Index == value.Index).FirstOrDefault();
            if (vCheck != null)
                throw new System.Exception("Mã tỉnh đã có trong hệ thống");
            value.Validate();
            return Request.SuccessResult(helper.Create(value, null));
        }

        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("DonviHanhchinh/Edit_Tinh")]
        [HttpPost]
        public HttpResponseMessage Edit_Tinh(uint id, [FromBody] MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh value)
        {
            value.Level = 0;
            value.Parent_id = 0;
            value.Validate();
            return Request.SuccessResult(helper.Update(id, value.DefaultUpdateDefine(), null, null));
        }

        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("DonviHanhchinh/Delete_Tinh")]
        [HttpPost]
        public HttpResponseMessage Delete_Tinh(uint id)
        {
            var vInfo = helper.FindById(id);
            if (vInfo == null)
                throw new System.Exception("Không tìm thấy thông tin cần xóa!");
            if (vInfo.Childs != null && vInfo.Childs.Count > 0)
                throw new System.Exception("Cần xóa hết danh sách huyện trước khi xóa tỉnh!");

            return Request.SuccessResult(helper.Delete(id));
        }
        #endregion
        #region Huyện
        /// <summary>
        /// Lấy danh sách huyện của tỉnh
        /// </summary>
        /// <param name="iTinh_id">ID tỉnh</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh>>))]
        [Route("DonviHanhchinh/Get_Huyen")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get_Huyen(uint iTinh_id)
        {
            var vTinh = helper.FindById(iTinh_id);
            if (vTinh != null)
                return Request.SuccessResult(vTinh.Childs.OrderBy(p => p.Name).ToList());
            else
                return Request.SuccessResult(new List<MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh>());
        }
        /// <summary>
        /// Thêm mới huyện
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<uint>))]
        [Route("DonviHanhchinh/Add_Huyen")]
        [HttpPost]
        public HttpResponseMessage Add_Huyen([FromBody] MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh value)
        {
            value.Level = 1;
            return Request.SuccessResult(helper.Add_Huyen(value));
        }
        /// <summary>
        /// Sửa thông tin huyện
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("DonviHanhchinh/Edit_Huyen")]
        [HttpPost]
        public HttpResponseMessage Edit_Huyen([FromBody] MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh value)
        {
            value.Level = 1;
            return Request.SuccessResult(helper.Edit_Huyen(value));
        }
        /// <summary>
        /// Xóa thông tin huyện
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("DonviHanhchinh/Delete_Huyen")]
        [HttpPost]
        public HttpResponseMessage Delete_Huyen([FromBody] MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh value)
        {
            var vInfo = helper.FindById(value.Parent_id);
            if (vInfo == null)
                throw new System.Exception("Không tìm thấy thông tin tỉnh!");
            if (vInfo.Childs != null)
            {
                var vItem = vInfo.Childs.Where(p => p.Id == value.Id).FirstOrDefault();
                if (vItem == null)
                    throw new System.Exception("Không tìm thấy thông tin cần xóa");
                var vCheck = new DonghoHelper(this.user.Id).Find(p => p.Huyen_id == value.Id).FirstOrDefault();
                if (vCheck != null)
                    throw new System.Exception("Huyện này đã có dữ liệu, không thể xóa");

                return Request.SuccessResult(helper.Update(value.Parent_id, p => p.Pull(a => a.Childs, vItem), null));

            }
            throw new System.Exception("Không tìm thấy thông tin cần xóa!");

        }
        /// <summary>
        /// Lấy danh mục huyện từ cổng thông tin quốc gia
        /// </summary>
        [ResponseType(typeof(APIResult<string>))]
        [Route("DonviHanhchinh/Convert_Huyen")]
        [HttpGet]
        public void Convert_Huyen()
        {
            DLQG_Huyen_Result DLQG_Huyen = GetDataAPI.GET<DLQG_Huyen_Result>(new GetDataAPIModel
            {

                url = "https://api.data.gov.vn/App/Home/api/v1/DmMeta/SelectOne/DanhMucVaMaSoCacDonViHanhChinhVietNamCap2capQuanHuyen",
            });
            foreach (var Item in DLQG_Huyen.data.listdataView)
            {
                DLQG_Huyen_Detail vItem = Newtonsoft.Json.JsonConvert.DeserializeObject<DLQG_Huyen_Detail>(Item.dataDetail);
                if (vItem != null && vItem.Masocaptinh == 12)
                {
                    var vTinh = helper.Find(p => p.Index == vItem.Masocaptinh).FirstOrDefault();
                    if (vTinh != null)
                    {
                        vTinh.Childs.Add(new MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh()
                        {
                            Id = Global.GetSequance<MongoDBAccess.Models.Danhmuc.Donvi_Hanhchinh>(),
                            Index = vItem.MaSoCapQuanHuyenThiXa,
                            Name = vItem.TenDonViHanhChinh,
                            Level = 1,
                            Parent_id = vTinh.Id,
                            Use = true
                        });
                        helper.Update(vTinh.Id, p => p.Set(a => a.Childs, vTinh.Childs), null);
                    }
                    else
                    {

                    }
                }

            }
        }
        #endregion
    }
}