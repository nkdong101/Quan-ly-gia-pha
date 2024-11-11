using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.API_Input
{
    public class Doi_lich
    {
        /// <summary>
        /// Ngày cần kiểm tra
        /// </summary>
        public DateTime Day { get; set; }
        /// <summary>
        /// True nếu đầu vào là ngày âm lịch
        /// </summary>
        public bool Amlich { get; set; }
        /// <summary>
        /// Dữ liệu đầu vào là ngày âm lịch 
        /// True: Nếu tháng đầu vào là tháng nhuận
        /// False: nếu tháng đầu vào không phải tháng nhuận
        /// </summary>
        public bool Nhuan { get; set; }
    }
}
