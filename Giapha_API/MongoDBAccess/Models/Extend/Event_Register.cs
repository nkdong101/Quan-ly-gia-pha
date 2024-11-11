using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Thông tin người đăng ký sự kiện
    /// </summary>
    public class Event_Register
    {
        /// <summary>
        /// Người đăng ký
        /// </summary>
        public uint UserId { get; set; }
        /// <summary>
        /// Ngày đăng ký
        /// </summary>
        public DateTime  DateActive { get; set; }
    }
}
