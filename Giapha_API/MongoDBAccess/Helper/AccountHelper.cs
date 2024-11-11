using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBAccess;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace MongoDBAccess
{
    /// <summary>
    /// Xử lý thông tin người dùng
    /// </summary>
    public class AccountHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<User>
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="iClient"></param>
        public AccountHelper(uint? iUserId, MongoClient iClient = null) : base(iUserId, iClient)
        {
        }
        /// <summary>
        /// Đăng ký sử dụng hệ thống
        /// </summary>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public void Register(API_Input.Register iInfo)
        {
            if (iInfo == null)
                throw new Exception("Dữ liệu đầu vào không đúng!");
            if (iInfo.Dongho_Info == null)
                throw new Exception("Vui lòng cung cấp thông tin về dòng họ của bạn!");
            if (iInfo.User_Info == null)
                throw new Exception("Vui lòng cung cấp thông tin cá nhân của bạn!");
            var vTinh = new DonviHanhchinhHelper(1).FindById(iInfo.Dongho_Info.Tinh_id);
            if (vTinh == null)
                throw new Exception("Vui lòng cung cấp tên tỉnh nơi dòng họ của bạn");
            iInfo.Dongho_Info.Address = vTinh.Name;
            var vHuyen = vTinh.Childs.Where(p => p.Id == iInfo.Dongho_Info.Huyen_id).FirstOrDefault();
            if (vHuyen != null)
                iInfo.Dongho_Info.Address = vHuyen.Name + " - " + iInfo.Dongho_Info.Address;
            iInfo.User_Info.Verify(null);
            iInfo.Dongho_Info.Validate();
            using (var session = this.MongoClient.StartSession())
            {
                try
                {
                    session.StartTransaction();
                    iInfo.User_Info.Id = Global.GetSequance<User>();
                    //1. Tạo dòng họ
                    var vHo_id = new DonghoHelper(iInfo.User_Info.Id, this.MongoClient).Add(iInfo.Dongho_Info, session);
                    //2. Tạo tài khoản
                    iInfo.User_Info.Dongho_id = vHo_id;
                    var vUser_id = this.AddUser(new User() { Id = 1, }, iInfo.User_Info, session);
                    //3. Tạo người đầu tiên trong dòng họ
                    var vGiaPha = new Gia_pha()
                    {
                        Address = iInfo.User_Info.Address,
                        Avatar = iInfo.User_Info.Images,
                        Dongho_id = vHo_id,
                        HoVietNam_id = iInfo.Dongho_Info.Ho_Vietnam_id,
                        Email = iInfo.User_Info.Email,
                        Name = iInfo.User_Info.FullName,
                        Birthday = iInfo.User_Info.BirthDay,
                        CCCD = iInfo.User_Info.CMND,
                        Gender = iInfo.User_Info.Gender,
                        Phone = iInfo.User_Info.Phone,
                        Year_Of_Birth = iInfo.User_Info.BirthDay.Year,
                        Use = true,
                        User_id = vUser_id,
                        State = Models.Enums.State.Live,
                    };
                    var vGiapha_id = new Gia_phaHelper(vUser_id, this.MongoClient).Create(vGiaPha, session);
                    // Cập nhật lại thông tin gia phả vào bảng users
                    this.Update(vUser_id, p => p.Set(a => a.Giapha_id, vGiapha_id), session, null);
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();
                    throw ex;
                }

            }
        }
        /// <summary>
        /// Sửa tài khoản
        /// </summary>
        /// <param name="iAccountAction"></param>
        /// <param name="iInfo"></param>
        /// <returns></returns>
        public string EditUser(User iAccountAction, User iInfo)
        {
            //Kiểm tra hợp lệ
            string vVerify = iInfo.Verify(iAccountAction);
            if (vVerify != "OK")
                throw new CreateExeption(vVerify);
            var result = this.Update(iInfo.Id, iInfo.DefaultUpdateDefine(), null);
            return result;
        }
        /// <summary>
        /// Thêm mới thông tin người dùng
        /// </summary>
        /// <param name="iAccountAction">Thông tin tài khoản thực hiện thêm dữ liệu</param>
        /// <param name="iInfo">Thông tin được thêm</param>
        /// <returns></returns>
        public uint AddUser(User iAccountAction, User iInfo, IClientSessionHandle iSession)
        {
            string vVerify = iInfo.Verify(iAccountAction);
            if (vVerify != "OK")
                throw new CreateExeption(vVerify);
            var vCheckUser = this.Find(p => p.UserName == iInfo.UserName).FirstOrDefault();
            if (vCheckUser != null)
                throw new Exception("Tài khoản đã tồn tại trong hệ thống!");
            var vCheckEmail = this.Find(p => p.Email == iInfo.Email && !string.IsNullOrEmpty(p.Email)).FirstOrDefault();
            if (vCheckUser != null)
                throw new Exception("Email này đã được sử dụng, vui lòng kiểm tra lại!");

            iInfo.Password = Security.EncryptPassword(iInfo.Password.Trim());

            //Kiểm tra hợp lệ

            uint result = this.Create(iInfo, iSession);
            return result;
        }


        /// <summary>
        /// Cập nhật thông tin quyền người dùng
        /// </summary>
        /// <param name="iPermission"></param>
        /// <param name="iId"></param>
        /// <returns></returns>
        public string SetPermission(GroupPermission iInfo, uint iId)
        {
            var vFind = this.FindById(iId);
            if (vFind != null)
            {
                vFind.Buttons = iInfo.Permission;
                List<UpdateDefinition<User>> vListUpdate = new List<UpdateDefinition<User>>();
                vListUpdate.Add(Builders<User>.Update.Set(p => p.Buttons, iInfo.Permission));
                vListUpdate.Add(Builders<User>.Update.Set(p => p.GroupPermission_Id, iInfo.Id));

                var vResult = this.Update(iId, vListUpdate, null);

                return vResult;
            }
            else
                return "Không tìm thấy thông tin tài khoản trong hệ thống!";
        }


        /// <summary>
        /// Bỏ
        /// Lấy danh sách Account by filter
        /// </summary>
        /// <param name="iFilters"></param>
        /// <returns></returns>
        public List<User> FindByFilter(AccountFilter iFilters)
        {
            var vResult = new List<User>();
            var builder = Builders<User>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(iFilters.Search))
                filter &= builder.Regex(a => a.FullName, new BsonRegularExpression($".*{iFilters.Search}.*"));
            if (iFilters.Dongho_id != null && iFilters.Dongho_id > 0)
                filter &= builder.Eq(a => a.Dongho_id, iFilters.Dongho_id);
            if (string.IsNullOrEmpty(iFilters.Search) && (iFilters.Dongho_id == null || iFilters.Dongho_id == 0))
                vResult = this.Find().SortBy(p => p.Dongho_id).ThenByDescending(p => p.DateCreate).Limit(100).ToList();
            else
                vResult = this.Find(filter).SortBy(p => p.DateCreate).ToList();
            var HoHelper = new DonghoHelper(this.UserId);
            Dong_ho vDongho = null;
            foreach (var Item in vResult)
            {
                if (vDongho == null || vDongho.Id != Item.Dongho_id)
                    vDongho = HoHelper.FindById(Item.Dongho_id);
                if (vDongho != null)
                    Item.Dongho_Name = vDongho.Name + $" ({vDongho.Address})";
            }
            return vResult;
        }
        /// <summary>
        /// Lấy thông tin user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User FindUser(string userName, string password)
        {
            User user = this.Find(p =>
               p.UserName.ToLower() == userName.ToLower().Trim()
            ).FirstOrDefault();

            if (user != null && user.Password == Security.EncryptPassword(password.Trim()))
                return user;
            return null;
        }

        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User ProcessLogin(string userName, string password)
        {
            User user = this.FindUser(userName, password);
            if (user == null)

                throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng.");
            return user;


        }
        /// <summary>
        /// Thay đổi ảnh đại diện
        /// </summary>
        /// <param name="iUser_id"></param>
        /// <param name="iURL"></param>
        public void ChangeAvatar(uint iUser_id, string iURL)
        {
            var vUser = this.FindById(iUser_id);
            if (vUser == null)
                throw new Exception("Không tìm thấy thông tin người dùng");
            this.Update(iUser_id, p => p.Set(a => a.Images, iURL), null);
        }

        /// <summary>
        /// Kiểm tra xóa tài khoản
        /// </summary>
        /// <param name="iId"></param>
        /// <param name="iUser"></param>
        /// <returns></returns>
        public string CheckDelete(uint iId, User iUser)
        {
            var result = this.Delete(iId);
            return result;
        }

    }


    /// <summary>
    /// Tham số Get đơn vị
    /// </summary>
    public class AccountFilter
    {
        /// <summary>
        /// Nội dung tìm kiếm
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// ID dòng họ
        /// </summary>
        public uint? Dongho_id { get; set; }
        /// <summary>
        /// ID tỉnh
        /// </summary>
        public uint? Tinh_id { get; set; }
        /// <summary>
        /// ID huyện
        /// </summary>
        public uint? Huyen_id { get; set; }
    }
}