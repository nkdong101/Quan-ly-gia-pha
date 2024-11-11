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
    public class Event_Registers_Helper : ModelGenericRepository<Models.Event_Registers>
    {
        public Event_Registers_Helper(uint? iUserId, MongoClient iClient = null) : base(iUserId, iClient)
        {
        }

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="iEvent_id">ID sự kiện</param>
        /// <returns></returns>
        public List<Models.Event_Registers> GetList(uint iEvent_id)
        {
            return this.Find(p => p.Event_id == iEvent_id).ToList();
        }
    }
}
