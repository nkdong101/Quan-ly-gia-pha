using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{

    public class XaPhuong : ObjectBase
    {
        /// <summary>
        /// Tên Khu vực
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Tên bỏ dấu
        /// </summary>
        public string slug { get; set; }
        /// <summary>
        /// Loại Khu vực
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Tên kèm với loại Khu vực
        /// </summary>
        public string name_with_type { get; set; }
        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// Tên đầy đủ với loại tỉnh thành, quận huyện, khu vực
        /// </summary>
        public string path_with_type { get; set; }
        /// <summary>
        /// Id tỉnh thành
        /// </summary>
        public string tinh_id { get; set; }
        /// <summary>
        /// Id quận huyện
        /// </summary>
        public string quan_id { get; set; }
    }





}