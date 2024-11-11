using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.API_Input
{
    /// <summary>
    /// Thông tin khi thêm con
    /// Khi Thêm con tất cả đều thêm từ mẹ (nếu không xác định được thì thêm bản ghi là không xác định)
    /// </summary>
    public class Add_Con_Input
    {
        /// <summary>
        /// Thông tin con
        /// </summary>
        public Models.Gia_pha Con_Info { get; set; }
        /// <summary>
        /// Thông tin mẹ 
        /// </summary>
        public Models.Gia_pha Me_Info { get; set; }
        /// <summary>
        /// True: Con riêng
        /// False: Con chung
        /// </summary>
        public bool Conrieng { get; set; }
    }
}
