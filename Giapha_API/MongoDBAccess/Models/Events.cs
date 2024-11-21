using MongoDBAccess.Models.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models
{
    /// <summary>
    /// Sự kiện
    /// </summary>
    public class Events : Objects.ObjectBase
    {
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Danh sách file đính kèm
        /// </summary>
        public List<string> Files { get; set; }
        /// <summary>
        /// Thời điểm bắt đầu
        /// </summary>
        public DateTime DateBegin { get; set; }
        /// <summary>
        /// Thời điểm kết thúc
        /// </summary>
        public DateTime DateEnd { get; set; }
        /// <summary>
        /// Thời điểm cuối cùng được phép đăng ký
        /// </summary>
        public DateTime? DateRegister { get; set; }
        ///// <summary>
        ///// Danh sách ban tổ chức
        ///// </summary>
        //public List<Event_Organizers> Organizers { get; set; }
        /// <summary>
        /// Trạng thái sự kiện
        /// </summary>
        public Enums.EventState State { get; set; }
        ///// <summary>
        ///// ID dòng họ
        ///// </summary>
        //public uint Dongho_id { get; set; }
        ///// <summary>
        ///// Số người tham gia (hệ thống tự tổng hợp khi người dùng đăng ký)
        ///// </summary>
        //public uint Person { get; set; }
        ///// <summary>
        ///// 3 người đăng ký gần nhất
        ///// </summary>
        //public List<Extend.Event_Register> Registers { get; set; }
        /// <summary>
        /// Kiểm tra dữ liệu
        /// </summary>
        public void Validate()
        {
           
            if (string.IsNullOrEmpty(this.Title))
                throw new Exception("Tiêu đề sự kiện không được để trống!");
            
        }
    }
}
