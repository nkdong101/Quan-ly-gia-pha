using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBAccess.Models
{
    public enum MenuResultType
    {
        /// <summary>
        /// Nút lệnh
        /// </summary>
        Button = 0,
        /// <summary>
        /// Menu
        /// </summary>
        Menu = 1,
        /// <summary>
        /// chức năng trong menu
        /// </summary>
        MenuAction = 2
    }

    public class MenuResult
    {

        public uint Id { get; set; }
        public string Icon { get; set; }

        public uint ParentId { get; set; }
      
        public MenuResultType Type_id { get; set; }
        /// <summary>
        /// True: Tất cả tài khoản đều có thể xem được
        /// </summary>

        public bool isViewAll { get; set; }

        public string Title { get; set; }

        public string Title_Class { get; set; }

        public string Tag { get; set; }

        public string Other_Class { get; set; }

        public string LiClass { get; set; }

        public string LiID { get; set; }

        public string Href { get; set; }

        public string Action { get; set; }

        public bool Expanded { get; set; }

        public ushort Stt { get; set; }

        public List<MenuResult> Child { get; set; }

        public List<MenuResult> ListButton { get; set; }

    }
}