using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models
{
    /// <summary>
    /// Thông tin góp ý
    /// </summary>
    public class Comments:Objects.ObjectBase
    {
        /// <summary>
        /// Người góp ý
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Số liên hệ
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// địa chỉ email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Nội dung góp ý
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Tài khoản góp ý
        /// </summary>
        public uint UserId { get; set; }
    }
}
