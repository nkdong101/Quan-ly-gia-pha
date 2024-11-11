using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Tham số cấu hình
    /// </summary>
    public class Tree_box_Config
    {
        /// <summary>
        /// Tọa độ bắt đầu vẽ
        /// </summary>
        public int X { get; set; } = 20;
        /// <summary>
        /// Tọa độ bắt đầu vẽ
        /// </summary>
        public int Y { get; set; } = 20;
        /// <summary>
        /// Chiều rộng ngoài cùng
        /// </summary>
        public int Width { get; set; } = 240;
        /// <summary>
        /// Chiều cao của khối
        /// </summary>
        public int Height { get; set; } = 120;
        /// <summary>
        /// Chiều rộng border
        /// </summary>
        public byte Border { get; set; } = 3;
        /// <summary>
        /// Khoảng cách giữa 2 khối
        /// </summary>
        public int Distance { get; set; } = 40;
        /// <summary>
        /// Chiều cao của Header
        /// </summary>
        public int Header_Height { get; set; } = 30;
        /// <summary>
        /// Kích thước khu vực vẽ
        /// </summary>
        public int MaxWidth { get; set; }
        /// <summary>
        /// Khu vực vẽ lớn nhất
        /// </summary>
        public int MaxHeight { get; set; }
    }
    /// <summary>
    /// Thông tin để vẽ
    /// </summary>
    public class Tree_Box
    {
        /// <summary>
        /// Cấp độ
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Thứ tự trong hàng
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Chiều ngang
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Chiều dọc
        /// </summary>
        public double Y { get; set; }
         
        public void Calc(Tree_box_Config iConfig)
        {
            this.X = (this.Index - 1) * (iConfig.Width + iConfig.Distance) + iConfig.X;
            this.Y = (this.Row - 1) * (iConfig.Height + iConfig.Distance) + iConfig.Y;
        }
        public Tree_Box(Tree_box_Config iConfig, int iRow, int iCol)
        {
            this.Row = iRow;
            this.Index = iCol;
            Calc(iConfig);
        }
        /// <summary>
        /// Xác định lại tọa độ
        /// </summary>
        /// <param name="iConfig"></param>
        /// <param name="iCol"></param>
        public void Re(Tree_box_Config iConfig, int iCol)
        {
            this.Index = iCol;
            Calc(iConfig);
        }
    }
}
