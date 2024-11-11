using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{

    public class QuanHuyen : ObjectBase
    {
        /// <summary>
        /// Tên Quận huyện
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Tên bỏ dấu
        /// </summary>
        public string slug { get; set; }
        /// <summary>
        /// Loại quận/huyện
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Tên kèm với loại quận/huyện
        /// </summary>
        public string name_with_type { get; set; }
        /// <summary>
        /// Tên đầy đủ 
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// Tên đầy đủ kèm với loại quận huyện, tỉnh thành
        /// </summary>
        public string path_with_type { get; set; }
        /// <summary>
        /// id tỉnh thành
        /// </summary>
        public string tinh_id { get; set; }
    }

}