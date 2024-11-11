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
    /// Xử lý nghiệp vụ thu chi
    /// </summary>
    public class Thuchi_Helper : ModelGenericRepository<Models.Thu_Chi>
    {
        public Thuchi_Helper(uint? iUserId, MongoClient iClient = null) : base(iUserId, iClient)
        {
        }

        /// <summary>
        /// Lấy danh sách theo sự kiện
        /// </summary>
        /// <param name="iEvent_id">ID sự kiện</param>
        /// <returns></returns>
        public List<Models.Thu_Chi> GetByEvent(uint iEvent_id)
        {
            return this.Find(p => p.Event_id == iEvent_id).ToList();
        }
        /// <summary>
        /// Thêm mới bản ghi thu chi
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public uint Add(Models.Thu_Chi iInfo)
        {
            iInfo.Validate();
            return this.Create(iInfo, null);
        }
        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Edit(Models.Thu_Chi iInfo)
        {
            iInfo.Validate();
            return this.Update(iInfo.Id, iInfo.DefaultUpdateDefine(), null);
        }
    }
}
