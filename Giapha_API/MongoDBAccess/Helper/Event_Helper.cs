using MongoDB.Driver;
using MongoDBAccess.DataAccess.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Helper
{
    /// <summary>
    /// Xử lý nghiệp vụ liên quan đến sự kiện
    /// </summary>
    public class Event_Helper : ModelGenericRepository<Models.Events>
    {
        public Event_Helper(uint? iUserId, MongoClient iClient = null) : base(iUserId, iClient)
        {
        }

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="iDongho_id">ID dòng họ lấy dữ liệu</param>
        /// <returns></returns>
        public List<Models.Events> GetList()
        {
            return this.Find().ToList();
        }
        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public uint Add(Models.Events iInfo)
        {
            iInfo.Validate();
            return this.Create(iInfo, null);
        }
        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Edit(Models.Events iInfo)
        {
            return this.Update(iInfo.Id, iInfo.DefaultUpdateDefine(), null);
        }
        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Remove(Models.Events iInfo)
        {
            return this.Delete(iInfo.Id);
        }
        /// <summary>
        /// Hủy sự kiện hoặc tạm dừng
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Cancel_Pending(Models.Events iInfo)
        {
            var vInfo = this.FindById(iInfo.Id);
            if (vInfo == null)
                throw new Exception("Không tìm thấy thông tin sự kiện!");
            if (iInfo.UserCreate != this.UserId)
                throw new Exception("Chỉ người tạo ra sự kiện mới được quyền thực hiện!");
            this.Update(iInfo.Id, p => p.Set(a => a.State, iInfo.State), null);
            return "OK";
        }
        /// <summary>
        /// Đăng ký tham gia sự kiện
        /// </summary>
        /// <param name="iInfo"></param>
        /// <param name="iUser"></param>
        /// <returns></returns>
        //public string Register(Models.Event_Registers iInfo, Models.User iUser)
        //{
        //    var vInfo = this.FindById(iInfo.Event_id);
        //    if (vInfo == null)
        //        throw new Exception("Không tìm thấy thông tin sự kiện!");
        //    if (vInfo.State != Models.Enums.EventState.Active)
        //        throw new Exception("Bạn chỉ có thể đăng ký khi sự kiện đang được kích hoạt!");
        //    if (vInfo.DateRegister != null && (DateTime)vInfo.DateRegister < DateTime.Now)
        //        throw new Exception("Rất tiếc, sự kiện đã hết hạn đăng ký!");

        //    using (var session = this.MongoClient.StartSession())
        //    {
        //        try
        //        {
        //            session.StartTransaction();
        //            var vRHelper = new Event_Registers_Helper(this.UserId, this.MongoClient);
        //            var vCheck = vRHelper.Find(p => p.Event_id == iInfo.Event_id && p.UserCreate == iUser.Id).FirstOrDefault();
        //            if (vCheck != null)
        //            {
        //                session.AbortTransaction();
        //                throw new Exception("Bạn đã đăng ký sự kiện này ngày: " + vCheck.DateCreate.ToString("dd/MM/yyyy"));
        //            }
        //            var vRegister = new Models.Event_Registers()
        //            {
        //                Event_id = vInfo.Id,
        //                Description = iInfo.Description
        //            };
        //            vRHelper.Create(vRegister, session);
        //            if (vInfo.Registers != null && vInfo.Registers.Count > 3)
        //            {
        //                this.Update(vInfo.Id, p => p.Pull(a => a.Registers, vInfo.Registers[0]), session);
        //            }
        //            this.Update(vInfo.Id, p => p.Push(a => a.Registers, new Models.Extend.Event_Register()
        //            {
        //                DateActive = DateTime.Now,
        //                UserId = iUser.Id
        //            }), null);

        //            session.CommitTransaction();
        //        }
        //        catch (Exception ex)
        //        {
        //            session.AbortTransaction();
        //            throw ex;
        //        }

        //    }
        //    return "OK";
        //}
    }
}
