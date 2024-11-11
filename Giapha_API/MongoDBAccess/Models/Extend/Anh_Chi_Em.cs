using MongoDBAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Thông tin anh chị em
    /// </summary>
    public class Anh_Chi_Em
    {
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public byte Index { get; set; }
        /// <summary>
        /// ID đối tượng
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// Mối quan hệ anh chị em
        /// </summary>
        public eQuanhe_ACE Quanhe { get; set; }
    }
}
