using MongoDBAccess.Models.Extend;
using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models
{
    /// <summary>
    /// Thông tin 1 cá nhân trong gia phả
    /// </summary>
    public class Gia_pha : Objects.ObjectBase
    {
        #region Thông tin cá nhân
        /// <summary>
        /// ID họ
        /// </summary>
        public uint Dongho_id { get; set; }
        ///// <summary>
        ///// ID họ việt nam
        ///// </summary>
        //public uint HoVietNam_id { get; set; }
        /// <summary>
        /// ID họ ngoại (chỉ áp dụng với nữ)
        /// Khi nữ lấy chồng thì họ sẽ theo nhà chồng và chuyển họ của mình thành họ ngoại
        /// Làm như thế để tạo mối liên hệ giữa 2 họ
        /// </summary>
        public uint Hongoai_id { get; set; }
        /// <summary>
        /// Họ tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tên gọi khác
        /// </summary>
        public string Other_Name { get; set; }
        /// <summary>
        /// Nơi sinh
        /// </summary>
        public string Place_of_birth { get; set; }
        /// <summary>
        /// Địa chỉ liên hệ hiện tại
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Nơi sinh
        /// </summary>
        public string Noisinh { get; set; }
        /// <summary>
        /// Số liên hệ
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Số CCCD
        /// </summary>
        public string CCCD { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// Thứ tự trong gia đình
        /// </summary>
        public byte Index { get; set; }
        /// <summary>
        /// Đời thứ mấy trong dòng họ
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public byte Level { get; set; }
        /// <summary>
        /// Năm sinh
        /// </summary>
        public int? Year_Of_Birth { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Đường link tư liệu mô tả về người này
        /// </summary>
        public List<string> URL { get; set; }


        /// <summary>
        /// tiểu sử 
        /// </summary>
        public string tieusu { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Enums.Gender Gender { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public Enums.State State { get; set; }
        #endregion
        #region Thông tin khi mất
        /// <summary>
        /// Ngày mất ÂL
        /// </summary>
        public DateTime? Date_of_death { get; set; }
        /// <summary>
        /// Năm mất ÂL
        /// </summary>
        public uint? year_die { get; set; }
        /// <summary>
        /// Nơi chôn cất sau khi mất
        /// </summary>
        public string burial_ground { get; set; }
        /// <summary>
        /// Tọa độ nơi chôn cất
        /// </summary>
        public Extend.Location Burial_GPS { get; set; }
        #endregion
        #region Thông tin phả hệ
        /// <summary>
        /// Thông tin bố mẹ
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public Pha_he Parent { get; set; } = new Pha_he();
        /// <summary>
        /// Danh sách con cái
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public List<Level> Childs { get; set; } = new List<Level>();
        /// <summary>
        /// Danh sách vợ/ chồng
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public List<Level> Couple { get; set; } = new List<Level>();
        /// <summary>
        /// Danh sách anh chị em
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public List<Anh_Chi_Em> siblings { get; set; } = new List<Anh_Chi_Em>();
        #endregion
        /// <summary>
        /// Tài khoản của người này trong hệ thống
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public uint User_id { get; set; }



        public DateTime DoilichAM_DUONG(DateTime day)
        {
           
            var vLich = new Tuvi.Lichvannien();
            int[] result = vLich.Amlich_Duonglich(day.Day, day.Month, day.Year,0);

            // result[0] -> Ngày âm lịch
            // result[1] -> Tháng âm lịch
            // result[2] -> Năm âm lịch

            try { 
            
                return new DateTime(result[2], result[1], result[0]);
            }
            catch (ArgumentOutOfRangeException) { 
            
                throw new InvalidOperationException("Ngày âm lịch không thể chuyển đổi thành DateTime.");
            }
        }


        /// <summary>
        /// Định dạng thành viên trong gia đình
        /// </summary>
        /// <returns></returns>
        public Extend.Family_Person Tofamily()
        {
            return new Family_Person()
            {
                Birthday = this.Birthday,
                Childs = new List<Family_Person>(),
                Id = this.Id,
                Index = this.Index,
                Avatar = this.Avatar,
                Gender = this.Gender,
                Name = this.Name,
                Other_Name = this.Other_Name,
                Year_Of_Birth = this.Year_Of_Birth,
                Date_of_death = this.Date_of_death == null ? (DateTime?)null : DoilichAM_DUONG(this.Date_of_death.Value),
                //Date_of_death_Lunar = this.Date_of_death == null ? (DateTime?)null : this.Doilich(this.Date_of_death.Value),
                Number_Childs = (byte)this.Childs.Count(),
                Number_Couple = (byte)this.Couple.Count(),
                Number_Siblings = (byte)(this.siblings.Count() + 1),
                Address = this.Address,
                burial_ground = this.burial_ground
            };
        }
        /// <summary>
        /// Định dạng dữ liệu hiển thị cây gia phả
        /// </summary>
        /// <returns></returns>
        public Extend.Giapha_Tree ToTree(Tree_Box iBox, Gia_pha iViewer)
        {
            string vName = this.Name;
            if (this.State == Enums.State.Dead)
            {
                var arr = vName.Split(new char[] { ' ' });
                vName = "Cụ " + arr[arr.Length - 1];
            }
            int iDoi = iViewer == null ? 0 : iViewer.Level - this.Level;
            var vInfo = Global.vList_LevelName.Where(p => p.Level == iDoi).ToList();
            string vTengoi = "";
            //if (vInfo.Count == 1)
            //    vTengoi = vInfo[0].Name;
            //else if (vInfo.Count == 0)
            //    vTengoi = iDoi > 0 ? "Tổ tiên" : "";
            //else
            //    vTengoi = vInfo.Where(p => p.Gender == this.Gender).Select(p => p.Name).FirstOrDefault();
            return new Models.Extend.Giapha_Tree()
            {
                id = this.Id,
                Level = this.Level,
                index = this.Index,
                fid = this.Parent.Father_id,
                mid = this.Parent.Mother_id,
                name = vName,
                Nammat = this.Date_of_death == null ? null : this.Date_of_death?.Year,
                //Date_of_death_Lunar = this.Date_of_death == null ? (DateTime?)null : this.Doilich(this.Date_of_death.Value),

                Namsinh = this.Year_Of_Birth,
                SL_Anhem = (byte)this.siblings.Count(),
                SL_Con = (byte)this.Childs.Where(a => a.Cognition).Count(),
                pids = this.Couple.OrderBy(a => a.Index).Select(a => a.Id).ToList(),
                State = this.State,
                Avatar = this.Avatar,
                gioitinh = this.Gender,
                User_id = this.User_id,
                Hongoai_id = this.Hongoai_id,
                Dongho_id = this.Dongho_id,
                Stt = iBox.Index,
                Box = iBox,
                Tengoi = vTengoi
            };
        }
    }
}
