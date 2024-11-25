using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Dữ liệu vẽ cây phả hệ
    /// </summary>
    public class Giapha_Tree
    {
        #region Properties

        /// <summary>
        /// ID hệ thống
        /// </summary>
        public uint id { get; set; }
        /// <summary>
        /// Đời
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Họ tên
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public Enums.Gender gioitinh { get; set; }
        /// <summary>
        /// ID mẹ
        /// </summary>
        public uint? mid { get; set; }
        /// <summary>
        /// ID cha
        /// </summary>
        public uint? fid { get; set; }
        /// <summary>
        /// Danh sách vợ/ chồng
        /// </summary>
        public List<uint> pids { get; set; }
        /// <summary>
        /// Con thứ mấy trong gia đình
        /// </summary>
        public byte index { get; set; }
        /// <summary>
        /// Số lượng anh em
        /// </summary>
        public byte SL_Anhem { get; set; }
        /// <summary>
        /// Số lượng con
        /// </summary>
        public byte SL_Con { get; set; }
        /// <summary>
        /// Stt khi sắp xếp
        /// </summary>
        public int Stt { get; set; }
        /// <summary>
        /// Năm sinh
        /// </summary>
        public int? Namsinh { get; set; }
        /// <summary>
        /// Năm mất
        /// </summary>
        public int? Nammat { get; set; }
        /// <summary>
        /// Ngày mất khi chuyển sang âm lịch
        /// </summary>
        public string Ngaymat_amlich { get; set; }
        /// <summary>
        /// Nam: luôn = 0
        /// Nữ: Họ của bố mẹ đẻ
        /// </summary>
        public uint Hongoai_id { get; set; }
        /// <summary>
        /// Nam: Họ nội
        /// Nữ: Họ của chồng
        /// </summary>
        public uint Dongho_id { get; set; }
        /// <summary>
        /// Thông số để vẽ cây gia phả
        /// </summary>
        public Tree_Box Box { get; set; }
        /// <summary>
        /// Tên gọi đối với người đang truy cập hệ thống
        /// </summary>
        public string Tengoi { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Enums.State State { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Avatar { get; set; }
        public uint? User_id { get; set; }
        #endregion
        #region Event

        #endregion
        #region Method
        /// <summary>
        /// Lấy tọa độ tâm bên phải hình vẽ
        /// </summary>
        /// <param name="iConfig"></param>
        /// <returns></returns>
        public Extend.Location GetRight(Tree_box_Config iConfig)
        {
            return new Location()
            {
                Lat = this.Box.X + iConfig.Width,
                Lng = this.Box.Y + iConfig.Height / 2
            };
        }
        /// <summary>
        /// Lấy tọa độ tâm bên trái hình vẽ
        /// </summary>
        /// <param name="iConfig"></param>
        /// <returns></returns>
        public Extend.Location GetLeft(Tree_box_Config iConfig)
        {
            return new Location()
            {
                Lat = this.Box.X,
                Lng = this.Box.Y + iConfig.Height / 2
            };
        }
        /// <summary>
        /// Tâm phía trên
        /// </summary>
        /// <param name="iConfig"></param>
        /// <returns></returns>
        public Extend.Location Center_Top(Tree_box_Config iConfig)
        {
            return new Location()
            {
                Lat = this.Box.X + iConfig.Width / 2,
                Lng = this.Box.Y
            };
        }
        #endregion

    }
}