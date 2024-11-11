using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBAccess.Models
{
    public class MenuButton: ObjectBase
    {
        #region Properties 
       
        private uint _Menu_id;
        public uint Menu_id
        {
            get { return _Menu_id; }
            set { _Menu_id = value; }
        }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Action;
        public string Action
        {
            get { return _Action; }
            set { _Action = value; }
        }
        private bool _Display;
        public bool Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
        private string _Element_Class;
        public string Element_class
        {
            get { return _Element_Class; }
            set { _Element_Class = value; }
        }
        private ushort _Stt;
        public ushort Stt
        {
            get { return _Stt; }
            set { _Stt = value; }
        }
        private string _SystemKey;
        public string Systemkey
        {
            get { return _SystemKey; }
            set { _SystemKey = value; }
        }
        #endregion
        #region Contructor 
        public MenuButton() { }

        #endregion
        #region Method 

        public MenuResult ToMenuResult()
        {
            MenuResult vResult = new MenuResult();
            vResult.Id = this.Id;
            vResult.Title = this.Name;
            vResult.Action = this.Action;
            vResult.LiClass = this.Element_class;
            vResult.Type_id = MenuResultType.Button;
            vResult.ParentId = this.Menu_id;
            vResult.Stt = this.Stt;
            vResult.Expanded = true;
            return vResult;
        }
        #endregion
    }
}