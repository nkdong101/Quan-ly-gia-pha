using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{

    public class TinhThanh : ObjectBase
    {
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Tên bỏ dấu
        /// </summary>
        public string slug { get; set; }
        /// <summary>
        /// Loại tỉnh thành
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Tên kèm với loại tỉnh thành
        /// </summary>
        public string name_with_type { get; set; }
    }

}