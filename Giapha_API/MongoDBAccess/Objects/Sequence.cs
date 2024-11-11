using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Objects
{
    /// <summary>
    /// Thông tin id cho từng bảng
    /// </summary>
    public class Sequence : ObjectBase
    {
        /// <summary>
        /// Giá trị lớn nhất
        /// </summary>
        public Int64 Value { get; set; }
        /// <summary>
        /// Tên bảng
        /// </summary>
        public string TableName { get; set; }
    }
}
