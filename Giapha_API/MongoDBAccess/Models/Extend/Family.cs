using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models.Extend
{
    /// <summary>
    /// Thông tin các thành viên trong gia đình
    /// </summary>
    public class Family
    {
        /// <summary>
        /// Thông tin bố
        /// </summary>
        public Family_Person Farther { get; set; }
        /// <summary>
        /// Thông tin mẹ
        /// </summary>
        public Family_Person Mother { get; set; }
        /// <summary>
        /// Thông tin chính mình
        /// </summary>
        public Family_Person Curent { get; set; }
        /// <summary>
        /// Thông tin vợ/ chồng
        /// </summary>
        public List<Family_Person> Couple { get; set; } = new List<Family_Person>();
        /// <summary>
        /// Thông tin anh chị em
        /// </summary>
        public List<Family_Person> Siblings { get; set; } = new List<Family_Person>();
    }
    /// <summary>
    /// Thông tin thành viên trong gia đình
    /// </summary>
    public class Family_Person
    {
        /// <summary>
        /// ID hệ thống
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// ảnh đại diện
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Tên thành viên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// tên gọi khác (nếu có)
        /// </summary>
        public string Other_Name { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public Enums.Gender Gender { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// Năm sinh
        /// </summary>
        public int? Year_Of_Birth { get; set; }
        /// <summary>
        /// Ngày mất
        /// </summary>
        public DateTime? Date_of_death { get; set; }
        /// <summary>
        /// Ngày mất ÂL
        /// </summary>
        public DateTime? Date_of_death_Lunar { get; set; }
        /// <summary>
        /// Con thứ mấy trong gia đình
        /// </summary>
        public byte Index { get; set; }
        /// <summary>
        /// Số lượng anh chị em trong gia đình
        /// </summary>
        public byte Number_Siblings { get; set; }
        /// <summary>
        /// Số lượng vợ
        /// </summary>
        public byte Number_Couple { get; set; }
        /// <summary>
        /// Số lượng con
        /// </summary>
        public byte Number_Childs { get; set; }
        /// <summary>
        /// Danh sách những người con dùng để hiển thị dạng tree
        /// </summary>
        public List<Family_Person> Childs { get; set; } = new List<Family_Person>();
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Nơi chôn cất
        /// </summary>
        public string burial_ground { get; set; }
        /// <summary>
        /// Thông tin mô tả
        /// </summary>
        public string Description
        {
            get
            {
                var vTen = this.Gender == Enums.Gender.female ? "Bà" : "Ông";
                string vResult = vTen;
                if (this.Number_Siblings > 1)
                    vResult += " là con " + (this.Index <= 1 ? "cả" : " thứ " + this.Index) + " trong gia đình có " + this.Number_Siblings + " anh chị em. ";
                else
                    vResult += " là con duy nhất trong gia đình. ";
                if (this.Number_Couple <= 0)
                    vResult += (vTen + " chưa xây dựng gia đình riêng");
                else if (this.Number_Couple == 1)
                    vResult += (vTen + " lập gia đình riêng");
                else
                    vResult += (vTen + " có " + this.Number_Couple + (this.Gender == Enums.Gender.female ? "chồng" : "vợ"));
                if (this.Number_Couple > 0 && this.Number_Childs == 0)
                    vResult += " nhưng không có con";
                else if (this.Number_Couple > 0)
                    vResult += (" và có " + this.Number_Childs + " người con");
                return vResult;
            }
        }
    }
}
