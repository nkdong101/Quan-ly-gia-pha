using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// Đối tượng trả về khi login thành công
    /// </summary>
    public class LoginResult
    {
        public uint? AccountSerial { get; set; }
        /// <summary>
        /// Tên tài khoản
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tài khoản đăng nhập
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Nội dung token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Loại tocken
        /// </summary>
        public string TokenType { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// Thời gian hết hạn (giây)
        /// </summary>
        public int ExpiresIn { get; set; }
        /// <summary>
        /// ID dòng họ
        /// </summary>
        public uint? Dongho_id { get; set; }
        /// <summary>
        /// Menu đăng nhập
        /// </summary>
        public List<MenuResult> Menu { get;  set; }
        public uint GroupPermission_Id { get;  set; }
        public uint userLevel { get; set; }
    }
}