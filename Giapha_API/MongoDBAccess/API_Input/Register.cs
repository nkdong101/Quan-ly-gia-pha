using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.API_Input
{
    /// <summary>
    /// Thông tin khi đăng ký tài khoản
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Thông tin người dùng
        /// </summary>
        public Models.User User_Info { get; set; }
        /// <summary>
        /// Thông tin dòng họ
        /// </summary>
        public Models.Dong_ho Dongho_Info { get; set; }
    }
}
