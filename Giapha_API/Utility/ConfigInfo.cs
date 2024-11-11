using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// THông tin cấu hình
    /// </summary>
    public class ConfigInfo
    {
        /// <summary>
        /// Tài khoản quản trị
        /// </summary>
        public static string UserAdmin => "admin";

        /// <summary>
        /// Mã bí mật 
        /// </summary>
        public static string SecretCode => "XYZ@135&&&";

        ///// <summary>
        ///// Thông tin cache user
        ///// </summary>
        //public static List<User> CacheUser { get; set; }

        //public static void LoadCache(bool isTest = false)
        //{
        //    //Lấy tất cả thông tin user
        //    CacheUser = new AccountHelper("1").Find().ToList();
        //}
        public static DateTime dateCheck = new DateTime(1900, 1, 1);
    }
}
