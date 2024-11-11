using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Thông tin cấp bậc
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public byte Index { get; set; }
        /// <summary>
        /// ID đối tượng
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// True: người thân cùng huyết thống (hoặc chính thống nếu là quan hệ vợ chồng)
        /// False: không cùng huyết thống
        /// </summary>
        public bool Cognition { get; set; }
    }
    public class Level_Index
    {
        /// <summary>
        /// Đời
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Số lượng bản ghi
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Thứ tự xử lý đến hiện tại
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Lấy stt lớn nhất trong đời
        /// </summary>
        /// <param name="iMax"></param>
        /// <returns></returns>
        public int GetIndex(int iMax)
        {
            this.Index += 1;
            if (iMax > this.Index)
                this.Index = iMax;
            return this.Index;
        }
    }
}
