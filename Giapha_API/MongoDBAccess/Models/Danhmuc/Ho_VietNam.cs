using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Danhmuc
{
    /// <summary>
    /// Dòng họ của việt nam
    /// </summary>
    public class Ho_VietNam : Objects.ObjectBase
    {
        /// <summary>
        /// Tên họ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Số lượng bản ghi
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public int Total { get; set; }
        /// <summary>
        /// Số lượng thành viên
        /// </summary>
        public int Person { get; set; }
        /// <summary>
        /// Danh sách tỉnh
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public List<uint> DS_Tinh { get; set; }
        /// <summary>
        /// Kiểm tra chuẩn hóa dữ liệu
        /// </summary>
        public void Validate()
        {
            this.Name = this.Name.ToLower().Trim();
            this.Name = this.Name.Substring(0, 1).ToUpper() + this.Name.Substring(1, this.Name.Length - 1);
        }
    }
}
