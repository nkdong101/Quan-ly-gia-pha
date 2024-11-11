using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models
{
    /// <summary>
    /// Người dùng đăng ký tham gia sự kiện
    /// </summary>
    public class Event_Registers:Objects.ObjectBase
    {
        /// <summary>
        /// Sự kiện đăng ký
        /// </summary>
        public uint Event_id { get; set; }
        /// <summary>
        /// Thông tin mô tả khi đăng ký
        /// </summary>
        public string Description { get; set; }
    }
}
