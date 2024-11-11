using MongoDB.Driver;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBAccess
{
    /// <summary>
    /// Họ của người dùng
    /// </summary>
    public class DonghoHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<Models.Dong_ho>
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="iUserId"></param>
        public DonghoHelper(uint? iUserId) : base(iUserId)
        {
        }
        public DonghoHelper(uint? iUserId, MongoClient iClient) : base(iUserId, iClient)
        {
        }
        /// <summary>
        /// thêm mới
        /// </summary>
        /// <param name="iInfo"></param>
        /// <param name="iSession"></param>
        /// <returns></returns>
        public uint Add(Dong_ho iInfo, IClientSessionHandle iSession)
        {
            uint vResult = this.Create(iInfo, iSession);
            var vHovietnam = new Ho_VietNamHelper(this.UserId, this.MongoClient);
            List<UpdateDefinition<Models.Danhmuc.Ho_VietNam>> vUpdate = new List<UpdateDefinition<Models.Danhmuc.Ho_VietNam>>();
            vUpdate.Add(Builders<Models.Danhmuc.Ho_VietNam>.Update.Inc(a => a.Total, 1));
            vHovietnam.Update(iInfo.Ho_Vietnam_id, vUpdate, iSession);
            return vResult;
        }
    }
}