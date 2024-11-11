using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.API_Input
{
    /// <summary>
    /// Dữ liệu đầu Vào khi thêm anh chị em
    /// </summary>
    public class Add_ACE_Input
    {
        /// <summary>
        /// Thông tin người thêm anh chị em
        /// </summary>
        public Models.Gia_pha Curent_Info { get; set; }
        /// <summary>
        /// Thông tin của anh chị em người thêm
        /// </summary>
        public Models.Gia_pha ACE_Info { get; set; }
    }
}
