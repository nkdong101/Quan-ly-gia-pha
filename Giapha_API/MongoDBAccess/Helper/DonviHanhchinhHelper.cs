using MongoDB.Driver;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBAccess
{
    /// <summary>
    /// Tỉnh - Thành phố
    /// </summary>
    public class DonviHanhchinhHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<Models.Danhmuc.Donvi_Hanhchinh>
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="iUserId"></param>
        public DonviHanhchinhHelper(uint? iUserId) : base(iUserId)
        {
        }
        /// <summary>
        /// Thêm mới huyện
        /// </summary>
        /// <param name="iHuyen"></param>
        /// <returns></returns>
        public uint Add_Huyen(Models.Danhmuc.Donvi_Hanhchinh iHuyen)
        {
            var vTinh = this.FindById(iHuyen.Parent_id);
            if (vTinh == null)
                throw new System.Exception("Không tìm thấy thông tin tỉnh!");
            iHuyen.Validate();
            iHuyen.Id = Global.GetSequance<Models.Danhmuc.Donvi_Hanhchinh>();
            if (vTinh.Childs == null)
            {
                vTinh.Childs = new List<Models.Danhmuc.Donvi_Hanhchinh>() { iHuyen };
                this.Update(iHuyen.Parent_id, p => p.Set(a => a.Childs, vTinh.Childs), null);
            }
            else
                this.Update(iHuyen.Parent_id, p => p.Push(a => a.Childs, iHuyen), null);
            return iHuyen.Id;
        }
        /// <summary>
        /// Sửa thông tin huyện
        /// </summary>
        /// <param name="iHuyen"></param>
        /// <returns></returns>
        public string Edit_Huyen(Models.Danhmuc.Donvi_Hanhchinh iHuyen)
        {
            var vTinh = this.FindById(iHuyen.Parent_id);
            if (vTinh == null)
                throw new System.Exception("Không tìm thấy thông tin tỉnh!");
            iHuyen.Validate();
            for (int i = 0; i < vTinh.Childs.Count; i++)
            {
                if (vTinh.Childs[i].Id == iHuyen.Id)
                {
                    vTinh.Childs[i].Name = iHuyen.Name;
                    vTinh.Childs[i].Index = iHuyen.Index;
                    vTinh.Childs[i].Use = iHuyen.Use;
                    break;
                }
            }
            this.Update(iHuyen.Parent_id, p => p.Set(a => a.Childs, vTinh.Childs), null);
            return "OK";
        }
    }
}