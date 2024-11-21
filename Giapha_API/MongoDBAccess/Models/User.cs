using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDBAccess;
using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

namespace MongoDBAccess.Models
{
    /// <summary>
    /// Thông tin người dùng
    /// </summary>
    public class User : ObjectBase
    {

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public string UserName { get; set; } = "";
        /// <summary>
        /// Mật khẩu (khi insert để giá trị chưa mã hóa)
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public string Password { get; set; }
        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Danh sách quyền
        /// </summary>
        [CustomObjectAttr(IsEdit = false)]
        public List<uint> Buttons { get; set; }
        /// <summary>
        /// Id nhóm quyền
        /// </summary>
        public uint GroupPermission_Id { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Images { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Số CCCD
        /// </summary>
        public string CMND { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// Số liên hệ
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public Enums.Gender Gender { get; set; } 
        /// <summary>
        /// Cấp độ người dùng (loại tài khoản)
        /// </summary>
        public uint userLevel { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ID dòng họ
        /// </summary>
        //public uint Dongho_id { get; set; }
        ///// <summary>
        ///// Tên dòng họ
        ///// </summary>
        //[BsonIgnore]
        //public string Dongho_Name { get; set; }
        /// <summary>
        /// ID người dùng trong gia phả
        /// </summary>
        public uint Giapha_id { get; set; }
        /// <summary>
        /// Kiểm tra tính hợp lên
        /// </summary>
        /// <param name="iAccountAction">Account thực hiện</param>
        /// <returns></returns>
        public string Verify(User iAccountAction)
        {
            if (string.IsNullOrEmpty(this.UserName))
                throw new Exception("Tên đăng nhập không được để trống!");
            if (string.IsNullOrEmpty(this.Password))
                throw new Exception("Mật khẩu đăng nhập không được để trống!");
            if (string.IsNullOrEmpty(this.FullName))
                throw new Exception("Họ tên người dùng không được để trống!");
            return "OK";
        }

        public List<MenuResult> GetMenu(bool isGetAll)
        {
            if (this.Buttons == null)
                this.Buttons = new List<uint>();
            List<MenuResult> vResult = new List<MenuResult>();
            try
            {
                List<MenuButton> vButton = new MenuButtonHelper(this.Id).Find().ToList();
                List<Menu> vData = new MenuHelper(this.Id).Find().ToList();
                List<Menu> vParentMenu = vData.Where(p => p.Parent_id == 0).OrderBy(p => p.Stt).ToList();
                foreach (Menu Item in vParentMenu)
                {

                    List<MenuResult> vListChild = Item.GetChild(this.UserName, this.Buttons, isGetAll, vData, vButton);
                    if (vListChild.Count > 0)
                    {
                        MenuResult ItemResult = Item.ToResult();
                        ItemResult.Child = vListChild;
                        ItemResult.ListButton = vButton.Where(p => this.Buttons.Contains(p.Id) && p.Menu_id == Item.Id).Select(p => p.ToMenuResult()).ToList();
                        vResult.Add(ItemResult);
                    }
                    else
                    {

                        List<MenuResult> vListButton = vButton.Where(p => this.Buttons.Contains(p.Id) && p.Menu_id == Item.Id).Select(p => p.ToMenuResult()).ToList();
                        if (vListButton.Count > 0)
                        {
                            MenuResult ItemResult = Item.ToResult();
                            ItemResult.ListButton = vListButton;
                            vResult.Add(ItemResult);
                        }
                        else if ((Item.Viewall && isGetAll) || this.UserName.ToUpper() == ConfigInfo.UserAdmin.ToUpper())
                        {
                            MenuResult ItemResult = Item.ToResult();
                            ItemResult.ListButton = vButton.Where(p => p.Menu_id == Item.Id).Select(p => p.ToMenuResult()).ToList();
                            vResult.Add(ItemResult);
                        }
                        else
                        {
                            MenuResult ItemResult = Item.ToResult();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return vResult.OrderBy(p => p.Stt).ToList();
        }
    }


}