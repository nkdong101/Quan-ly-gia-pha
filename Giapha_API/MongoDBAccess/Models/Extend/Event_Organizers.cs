using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Danh sách thành viên ban tổ chức sự kiện
    /// </summary>
    public class Event_Organizers
    {
        /// <summary>
        /// ID người tham gia
        /// </summary>
        public string User_id { get; set; }
        /// <summary>
        /// Vai trò trong ban tổ chức
        /// </summary>
        public string Vaitro { get; set; }
        /// <summary>
        /// Mô tả thông tin khác
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int Index { get; set; }
    }
}
