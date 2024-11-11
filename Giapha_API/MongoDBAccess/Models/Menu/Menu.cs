using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

namespace MongoDBAccess.Models
{
    public class Menu : ObjectBase
    {
        /// <summary>
        /// Danh sách nút lệnh trong từng menu
        /// </summary>
        public List<MenuButton> ListMenuButton { get; set; }
        /// <summary>
        /// Danh sách nút lệnh dùng hiển thị trên web (lấy theo người dùng)
        /// </summary>
        public List<MenuButton> ListButton { get; set; }
        #region Properties 
        /// <summary>
        /// Menu cấp trên
        /// </summary>
        public uint Parent_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Tên menu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string Fullname { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// True: Ai cũng có thể xem
        /// False: Chỉ người được cấp quyền mới có thể xem
        /// </summary>
        public bool Viewall { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Element_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Element_class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title_class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Other_class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UInt16 Stt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Systemkey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MenuResultType Type_id { get; set; }
        #endregion
        #region Contructor 
        public Menu()
        {
            this.ListMenuButton = new List<MenuButton>();
            this.ListButton = new List<MenuButton>();
        }
        public Menu Copy(string iListButton)
        {
            Menu vResult = (Menu)this.MemberwiseClone();
            if (iListButton != null && iListButton.Trim() != "")
                vResult.ListButton = this.ListMenuButton.Where(p => iListButton.Contains(string.Format(",{0},", p.Id))).ToList();
            return vResult;
        }
        public void AddButton(MenuButton iItem)
        {
            if (this.ListMenuButton.Where(p => p.Id == iItem.Id).Count() == 0)
                this.ListMenuButton.Add(iItem);
            else
            {
                for (int i = 0; i < this.ListMenuButton.Count; i++)
                {
                    if (this.ListMenuButton[i].Id == iItem.Id)
                    {
                        this.ListMenuButton[i] = iItem;
                        break;
                    }
                }
            }
        }
        public string SaveMenuButton(MenuButton iInfo)
        {
            try
            {
                //if (iInfo.Id == 0)
                //{
                //    string vResult = iInfo.SaveInfo(true);
                //    ushort vID = 0;
                //    ushort.TryParse(vResult, out vID);
                //    if (vID > 0)
                //    {
                //        iInfo.Id = vID;
                //        ListMenuButton.Add(iInfo);
                //        return "OK";
                //    }
                //    return vResult;
                //}
                //else
                //{
                //    if (ListMenuButton.Where(p => p.Id == iInfo.Id).Count() == 0)
                //        return "Nút lệnh này không còn tồn tại trong hệ thống!";
                //    string vResult = iInfo.SaveInfo(false);
                //    if (vResult == "OK")
                //        ListMenuButton[iInfo.Id] = iInfo;
                //    return vResult;
                //}
                return "";
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }
        public MenuResult ToResult()
        {
            MenuResult vResult = new MenuResult();
            vResult.Action = this.Action;
            vResult.Href = this.Controller;
            vResult.Id = this.Id;
            vResult.Icon = this.Icon;
            vResult.LiClass = this.Element_class;
            vResult.LiID = this.Element_id;
            vResult.Other_Class = this.Other_class;
            vResult.Tag = this.Tag;
            vResult.Title = this.Fullname;
            vResult.Title_Class = this.Title_class;
            vResult.ParentId = this.Parent_id;
            vResult.Stt = this.Stt;
            vResult.isViewAll = this.Viewall;
            vResult.Expanded = true;
            vResult.Child = new List<MenuResult>();
            vResult.ListButton = new List<MenuResult>();
            vResult.Type_id = this.Type_id;
            return vResult;
        }
        public List<MenuResult> GetChild(string iUserName, List<uint> iListButton, bool isGetAll, List<Menu> iData, List<MenuButton> vButton)
        {
            List<MenuResult> vResult = new List<MenuResult>();
            List<Menu> vParent = iData.Where(p => p.Parent_id == this.Id).OrderBy(p => p.Stt).ToList();
            foreach (Menu Item in vParent)
            {
                List<MenuResult> vListChild = Item.GetChild(iUserName, iListButton, isGetAll, iData, vButton);
                if (vListChild.Count > 0)
                {
                    MenuResult ItemResult = Item.ToResult();
                    ItemResult.Child = vListChild;
                    if (Item.Viewall)
                        ItemResult.ListButton = vButton.Where(p => p.Menu_id == Item.Id).OrderBy(p => p.Stt).Select(p => p.ToMenuResult()).ToList();
                    else
                        ItemResult.ListButton = vButton.Where(p => p.Menu_id == Item.Id).Where(p => iListButton.Contains(p.Id)).OrderBy(p => p.Stt).Select(p => p.ToMenuResult()).ToList();
                    vResult.Add(ItemResult);
                }
                else
                {
                    if ((Item.Viewall && isGetAll) || iUserName.ToUpper() == ConfigInfo.UserAdmin.ToUpper())
                    {
                        MenuResult ItemResult = Item.ToResult();
                        ItemResult.ListButton = vButton.Where(p => p.Menu_id == Item.Id).OrderBy(p => p.Stt).Select(p => p.ToMenuResult()).ToList();
                        vResult.Add(ItemResult);
                    }
                    else
                    {
                        List<MenuResult> vListButton = vButton.Where(p => p.Menu_id == Item.Id).Where(p => iListButton.Contains(p.Id)).OrderBy(p => p.Stt).Select(p => p.ToMenuResult()).ToList();
                        if (vListButton.Count > 0)
                        {
                            MenuResult ItemResult = Item.ToResult();
                            ItemResult.ListButton = vListButton;
                            vResult.Add(ItemResult);
                        }
                    }
                }

            }
            return vResult;
        }
        public void Update(Menu iNew)
        {
            this.Element_class = iNew.Element_class;
            this.Element_id = iNew.Element_id;
            this.Fullname = iNew.Fullname;
            this.Icon = iNew.Icon;
            this.Action = iNew.Action;
            this.Name = iNew.Name;
            this.Other_class = iNew.Other_class;
            this.Parent_id = iNew.Parent_id;
            this.Stt = iNew.Stt;
            this.Tag = iNew.Tag;
            this.Title_class = iNew.Title_class;
            this.Viewall = iNew.Viewall;
            this.Controller = iNew.Controller;
            this.Type_id = iNew.Type_id;
        }
        #endregion
        public void UpdateInfo(Menu iNew)
        {
            this.Controller = iNew.Controller;
            this.Element_class = iNew.Element_class;
            this.Element_id = iNew.Element_id;
            this.Fullname = iNew.Fullname;
            this.Icon = iNew.Icon;
            this.Action = iNew.Action;
            this.Name = iNew.Name;
            this.Other_class = iNew.Other_class;
            this.Parent_id = iNew.Parent_id;
            this.Stt = iNew.Stt;
            this.Systemkey = iNew.Systemkey;
            this.Tag = iNew.Tag;
            this.Title_class = iNew.Title_class;
            this.Viewall = iNew.Viewall;
            this.Type_id = iNew.Type_id;
        }
    }
}