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
    public class ThongBao
    {
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public byte Id { get; set; }
        /// <summary>
        /// Tên đối tượng
        /// </summary>
        public uint Name { get; set; }
        /// <summary>
        /// Mối quan hệ anh chị em
        /// </summary>
        public List<Models.Comments> Childs { get; set; }
    }
}
