using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models
{
    /// <summary>
    /// Thông tin dòng họ do người dùng đăng ký
    /// </summary>
    public class Dong_ho : Objects.ObjectBase
    {
        /// <summary>
        /// Tên dòng họ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ID dòng họ việt nam
        /// </summary>
        public uint Ho_Vietnam_id { get; set; }
        /// <summary>
        /// ID tỉnh hiện tại
        /// </summary>
        public uint Tinh_id { get; set; }
        /// <summary>
        /// ID Huyện
        /// </summary>
        public uint Huyen_id { get; set; }
        /// <summary>
        /// Thông tin mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Địa chỉ dòng họ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Tổng số con cháu (hệ thống tự tính)
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public int Total { get; set; }
        /// <summary>
        /// Tổng số xuất đinh (nam giới và còn sống)
        /// </summary>
        public int Total_Male { get; set; }
        /// <summary>
        /// Kiểm tra dữ liệu
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
                throw new Exception("Tên dòng họ không được để trống!");
            if (this.Tinh_id == 0)
                throw new Exception("Vui lòng cho chúng tôi biết dòng họ của bạn ở tỉnh nào?");
            if (this.Huyen_id == 0)
                throw new Exception("Vui lòng cho chúng tôi biết dòng họ của bạn ở huyện nào?");
            if (this.Ho_Vietnam_id == 0)
                throw new Exception("Vui lòng cho chúng tôi biết họ của bạn thuộc dòng họ nào của nước ta?");
        }
    }
}
