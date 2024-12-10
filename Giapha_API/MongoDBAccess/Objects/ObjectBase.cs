using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Objects
{

    public class ObjectBase
    {
        /// <summary>
        /// Id hệ thống (hệ thống lấy tự động)
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public uint Id { get; set; }
        /// <summary>
        /// Người tạo (hệ thống lấy tự động)
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public uint UserCreate { get; set; }
        /// <summary>
        /// Người cập nhật (hệ thống lấy tự động)
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public uint? UserUpdate { get; set; }
        /// <summary>
        /// Ngày tạo (hệ thống lấy tự động)
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [CustomObjectAttr(IsEdit = false)]
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Ngày cập nhật (hệ thống lấy tự động)
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [CustomObjectAttr(IsEdit = false)]
        public DateTime? DateUpdate { get; set; }

    }
}
