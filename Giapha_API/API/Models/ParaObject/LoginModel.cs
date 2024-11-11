using API.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Utility;

namespace API.Models
{
    /// <summary>
    /// Tham số cần truyền
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string iUsername { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string iPassword { get; set; }
    }

    public class LoginUlti
    {
        public uint Id { get; set; }
        public LoginUlti(uint iUserId)
        {
            this.Id = iUserId;
        }

        public string UserName { get; set; }
        /// <summary>
        /// domain
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime DateExpired { get; set; }
        /// <summary>
        /// Chuỗi Mã hóa
        /// </summary>
        public string Hash { get; set; }
        /// <summary>
        /// Kiểm tra mật khẩu có đúng với mã hóa khi đăng nhập không
        /// </summary>
        /// <param name="iPassword"></param>
        /// <returns></returns>
        public string GenericHash(string iPassword)
        {
            return Security.EncryptPassword(this.UserName + ";" + iPassword + ";" + this.Authority + ";" + this.DateExpired);
        }
        /// <summary>
        /// Lấy chuỗi token
        /// </summary>
        /// <param name="iPassword"></param>
        /// <returns></returns>
        public string GetAccessToken(string iPassword)
        {
            this.DateExpired = DateTime.Now.AddDays(1);
            this.Hash = GenericHash(iPassword);
            return Security.Encrypt(JSON.Stringify(this), true); ;
        }
        /// <summary>
        /// Định dạng dữ liệu từ token
        /// </summary>
        /// <param name="iToken"></param>
        /// <returns></returns>
        public LoginUlti FromAccessToken(string iToken)
        {
            string vToke = Security.Decrypt(iToken, true);
            var vData = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginUlti>(vToke);
            return vData;
        }
    }

}