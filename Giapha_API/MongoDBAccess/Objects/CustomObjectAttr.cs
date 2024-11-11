using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Objects
{
    public class CustomObjectAttr: Attribute
    {
        /// <summary>
        /// Cho phép trường được sử đụng trong hàm thêm hoặc sửa khi sử dụng hàm DefaultUpdateDefine()
        /// true: cho phép thêm/ Sửa (default)
        /// false: không cho phép thêm/ Sửa
        /// </summary>
        public bool IsEdit { get; set; } = true;
    }
}
