using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models
{
    /// <summary>
    /// Quản lý thu chi trong dòng họ
    /// </summary>
    public class Thu_Chi : Objects.ObjectBase
    {
        /// <summary>
        /// ID dòng họ
        /// </summary>
        public uint Dongho_id { get; set; }
        /// <summary>
        /// True: Thu
        /// False: Chi
        /// </summary>
        public bool isThu { get; set; }
        /// <summary>
        /// Mô tả về khoản thu/ chi
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Thời điểm ghi nhận
        /// </summary>
        public DateTime DateActive { get; set; }
        /// <summary>
        /// True: Chuyển khoản
        /// False: Tiền mặt
        /// </summary>
        public bool Bank { get; set; }
        /// <summary>
        /// người thực hiện
        /// </summary>
        public string Person { get; set; }
        /// <summary>
        /// Số tiền (nếu chi thì số tiền để âm, sau này cộng tổng cho dễ)
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// ID sự kiện nếu thu chi cho sự kiện
        /// </summary>
        public uint? Event_id { get; set; }
        /// <summary>
        /// kiểm tra tính hợp lệ của dữ liệu
        /// </summary>
        public void Validate()
        {
            if (this.Money == 0)
                throw new Exception("Số tiền không thể để = 0");
            if ((this.isThu && this.Money < 0) || (!this.isThu && this.Money > 0))
                this.Money = this.Money * -1;

        }
    }
}
