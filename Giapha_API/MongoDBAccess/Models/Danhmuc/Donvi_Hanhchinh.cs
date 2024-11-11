using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Danhmuc
{
    /// <summary>
    /// Thông tin tỉnh
    /// </summary>
    public class Donvi_Hanhchinh : Objects.ObjectBase
    {
        /// <summary>
        /// Cấp độ
        /// 0: = Tỉnh
        /// 1: = Huyện
        /// 2: = Xã
        /// </summary>
        public byte Level { get; set; }
        /// <summary>
        /// Tên tỉnh
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mã số
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// ID cấp trên
        /// </summary>
        public uint Parent_id { get; set; }
        /// <summary>
        /// Danh sách đơn vị hành chính cấp dưới
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public List<Donvi_Hanhchinh> Childs { get; set; } = new List<Donvi_Hanhchinh>();
        public void Validate()
        {
            this.Name = Utility.TextManage.ChuHoaDau(this.Name);
        }
    }
}
