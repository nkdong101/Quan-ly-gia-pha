using MongoDB.Driver;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBAccess
{
    /// <summary>
    /// Họ việt nam
    /// </summary>
    public class Ho_VietNamHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<Models.Danhmuc.Ho_VietNam>
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="iUserId"></param>
        public Ho_VietNamHelper(uint? iUserId, MongoClient iClient = null) : base(iUserId,iClient)
        {
        }
    }
}