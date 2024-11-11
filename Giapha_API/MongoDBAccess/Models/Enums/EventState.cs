using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Enums
{
    /// <summary>
    /// Trạng thái đăng ký sự kiện
    /// </summary>
    public enum EventState : byte
    {
        /// <summary>
        /// Chưa kích hoạt
        /// </summary>
        None = 1,
        /// <summary>
        /// Đang kích hoạt
        /// </summary>
        Active = 2,
        /// <summary>
        /// Bị hủy
        /// </summary>
        Cancel = 3,
        /// <summary>
        /// Tạm dừng 
        /// </summary>
        Pending =4,
        /// <summary>
        /// Kết thúc
        /// </summary>
        Finish = 5
    }
}
