﻿using MongoDB.Driver;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBAccess
{
    /// <summary>
    /// Góp ý của người dùng
    /// </summary>
    public class CommentHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<Models.Comments>
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="iUserId"></param>
        public CommentHelper(uint? iUserId) : base(iUserId)
        {
        }
        public CommentHelper(uint? iUserId, MongoClient iClient) : base(iUserId, iClient)
        {
        }
        /// <summary>
        /// thêm mới
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string Add(Comments iInfo)
        {
            uint vResult = this.Create(iInfo, null);
            return "Góp ý của bạn đã được chúng tôi ghi nhận, xin cảm ơn và chúc bạn mọi điều tốt lành!";
        }

        public List<Models.Comments> GetList()
        {
            return this.Find().ToList();
        }


        public List<Models.Comments> GetNoti()
        {
            return this.Find().SortByDescending(p=>p.DateCreate).Limit(7).ToList();
        }

        //public Models.Extend.ThongBao GetThongBao()
        //{

        //}

    }
}