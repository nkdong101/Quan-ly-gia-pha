using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.API_Input
{
    /// <summary>
    /// Thêm mới vợ/ chồng
    /// </summary>
    public class Add_Vo_Input
    {
        /// <summary>
        /// Thông tin người chồng thực hiện thêm vợ
        /// </summary>
        public Models.Gia_pha Chong_Info { get; set; }
        /// <summary>
        /// Thông tin Vợ/ chồng
        /// </summary>
        public Models.Gia_pha Vo_Info { get; set; }
        /// <summary>
        /// Thứ tự của vợ (vợ thứ mấy?)
        /// </summary>
        public byte Index { get; set; }
    }
}
