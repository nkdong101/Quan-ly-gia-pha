using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Enums
{
    /// <summary>
    /// Trạng thái 
    /// </summary>
    public enum State : byte
    {
        /// <summary>
        /// Đang sống
        /// </summary>
        Live = 1,
        /// <summary>
        /// Đã chết
        /// </summary>
        Dead = 2,
        /// <summary>
        /// Hiện không có thông tin
        /// </summary>
        No_Infomation = 2
    }
}
