using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// Code trả về khi call API
    /// </summary>
    public enum APIResultCode
    {
        /// <summary>
        /// Thành công
        /// </summary>
        SUCCESS = 1,
        /// <summary>
        /// Lỗi
        /// </summary>
        ERROR = 2,
        /// <summary>
        /// Chưa đăng nhập
        /// </summary>
        Auth = 3,
    }

    /// <summary>
    /// Đối tượng api trả về
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResult<T>
    {
        /// <summary>
        /// Result code trả về
        /// </summary>
        public APIResultCode Code { get; set; }
        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string CodeName => this.Code.ToString();
        /// <summary>
        /// Nội dung lỗi
        /// </summary>
        public string ErrMessage { get; set; }
        /// <summary>
        /// Data trả về
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="mesage"></param>
        /// <param name="data"></param>
        public APIResult(APIResultCode code, string mesage, T data)
        {
            this.Code = code;
            this.ErrMessage = mesage;
            this.Data = data;
        }


        /// <summary>
        /// Error result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public APIResult(APIResultCode code, string message)
        {
            this.Code = code;
            this.ErrMessage = message;
        }

        /// <summary>
        /// Success result
        /// </summary>
        /// <param name="data"></param>
        public APIResult(T data)
        {
            this.Code = APIResultCode.SUCCESS;
            this.ErrMessage = null;
            this.Data = data;
        }

        internal bool isError()
        {
            return this.Code != APIResultCode.SUCCESS;
        }
    }
}