using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Thông tin hiển thị trang chủ
    /// </summary>
    public class HomeInfo
    {
        /// <summary>
        /// Số lượng dòng họ
        /// </summary>
        public long Dongho { get; set; }
        /// <summary>
        /// Số lượng người trong gia phả
        /// </summary>
        public long Giapha { get; set; }
        /// <summary>
        /// Số tỉnh có mặt
        /// </summary>
        public int Tinh { get; set; }
    }
}
