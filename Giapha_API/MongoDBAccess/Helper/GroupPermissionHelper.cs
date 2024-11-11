using MongoDB.Driver;
using MongoDBAccess.Models;
using System;
using System.Linq;


namespace MongoDBAccess
{
    /// <summary>
    /// Xử lý dữ liệu đơn vị cung cấp bảo hiểm
    /// </summary>
    public class GroupPermissionHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<GroupPermission>
    {
        /// <summary>
        /// Khởi tạo giá trị
        /// </summary>
        /// <param name="iUserId"></param>
        public GroupPermissionHelper(uint? iUserId) : base(iUserId)
        {
        }

        public string ProcessUpdate(uint id, GroupPermission value)
        {
            this.Update(id, value.DefaultUpdateDefine(), null);

            AccountHelper accountHelper = new AccountHelper(1);
            //Update user với nhóm quyền cũ về mới
            accountHelper.Collection.UpdateMany(p => p.GroupPermission_Id == id, Builders<User>.Update.Set(p => p.Buttons, value.Permission));
            //Load lại cache
            return "OK";
        }

        public string ProcessDelete(uint id)
        {

            AccountHelper accountHelper = new AccountHelper(1);
            var total = accountHelper.Find(p => p.GroupPermission_Id == id).ToList();
            if (total.Count > 0)
            {
                throw new Exception("Bạn không thể xóa nhóm quyền vì đang có tài khoản sử dụng nhóm quyền này: " + string.Join(",", total.Select(p => p.FullName + " - " + p.CMND)));
            }

            return this.Delete(id);
        }
    }


}