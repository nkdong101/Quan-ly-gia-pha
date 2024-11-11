using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Enums
{
    /// <summary>
    /// Quan hệ gia đình
    /// </summary>
    public enum Quanhe : byte
    {
        /// <summary>
        /// Bố hoặc mẹ
        /// </summary>
        Bo_Me = 1,
        /// <summary>
        /// Vợ hoặc chồng
        /// </summary>
        Vo_Chong = 2,
        /// <summary>
        /// Con đẻ
        /// </summary>
        Con_de = 3,
        /// <summary>
        /// Con nuôi
        /// </summary>
        Con_nuoi = 4,
        /// <summary>
        /// Anh hoặc chị
        /// </summary>
        Anh_chi = 5,
        /// <summary>
        /// Em
        /// </summary>
        Em = 6
    }
}
