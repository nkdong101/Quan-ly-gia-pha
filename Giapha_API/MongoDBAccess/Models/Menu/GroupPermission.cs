using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBAccess.Models
{
    public class GroupPermission : ObjectBase
    {
        /// <summary>
        /// Tên nhóm quyền
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        
    }
}