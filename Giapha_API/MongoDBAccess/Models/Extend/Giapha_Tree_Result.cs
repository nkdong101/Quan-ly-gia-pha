using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    public class Giapha_Tree_Result
    {
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public List<Giapha_Tree> Data { get; set; } = new List<Giapha_Tree>();
        /// <summary>
        /// Thông số cấu hình
        /// </summary>
        public Extend.Tree_box_Config Config { get; set; }
        /// <summary>
        /// Danh sach duong ves
        /// </summary>
        public List<Tree_Line> Lines = new List<Tree_Line>();
    }
    /// <summary>
    /// Thông tin đoạn nối
    /// </summary>
    public class Tree_Line
    {
        /// <summary>
        /// ID 1
        /// </summary>
        public uint Id1 { get; set; }
        /// <summary>
        /// id 2
        /// </summary>
        public uint Id2 { get; set; }
        /// <summary>
        /// Danh sách tọa độ
        /// </summary>
        public string Points { get; set; }
    }
}
