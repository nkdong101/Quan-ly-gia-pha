using API.Models;
using MongoDB.Driver;
using MongoDBAccess;
//using MongoDBAccess.Google;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using Utility;


namespace API.Controllers
{
    [CustomAuthorization]
    public class AccountController : BaseController
    {
        private AccountHelper _helper;
        public AccountHelper helper
        {
            get
            {
                if (_helper == null)
                    _helper = new AccountHelper(this.user?.Id);
                return _helper;
            }
        }
        /// <summary>
        /// Đăng ký sử dụng
        /// </summary>
        /// <param name="Value">Thông tin đăng ký</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Account/Register")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Register([FromBody] MongoDBAccess.API_Input.Register Value)
        {
            helper.Register(Value);
            return Request.SuccessResult("OK");

        }
        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<LoginResult>))]
        [Route("Account/Login")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Login([FromBody] LoginModel model)
        {
            var user = helper.ProcessLogin(model.iUsername, model.iPassword);
            LoginUlti login = new LoginUlti(user.Id) { Authority = Request.RequestUri.Authority, UserName = user.UserName };
            List<GroupPermission> groupPermissions = new GroupPermissionHelper(this.user?.Id).Find().ToList();
            return Request.SuccessResult(new LoginResult
            {
                AccessToken = login.GetAccessToken(user.Password),
                AccountSerial = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                //GroupPermission_Id = user.GroupPermission_Id,
                //Menu = groupPermissions.Where(x => x.Id == user.userLevel).FirstOrDefault().Permission.ToList(),
                userLevel = user.userLevel,
                ImgUrl = user.Images,
                TokenType = "Bearer",
                Dongho_id = 1,
                ExpiresIn = (int)login.DateExpired.Subtract(DateTime.Now).TotalSeconds
            });
        }
        /// <summary>
        /// Đăng nhập với tài khoản gmail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[ResponseType(typeof(APIResult<LoginResult>))]
        //[Route("Account/LoginGoogle")]
        //[HttpPost]
        //[AllowAnonymous]
        //public HttpResponseMessage LoginGoogle([FromBody] string model)
        //{
        //    string redirect_uri = Request.Headers.GetValues("redirect_uri").FirstOrDefault();
        //    LoginSocial loginSocial = GoogleAuthPortal.Login(model, redirect_uri);
        //    if (loginSocial == null)
        //        throw new Exception("Đăng nhập không thành công");

        //    User user = new AccountHelper(1).Find(p => p.Email.ToLower() == loginSocial.email.ToLower()).FirstOrDefault();
        //    if (user == null)
        //        throw new Exception("Tài khoản của bạn không có trên hệ thống");

        //    if (!user.Use)
        //    {
        //        throw new Exception("Tài khoản của bạn đã bị khóa");
        //    }

        //    LoginUlti login = new LoginUlti(user.Id) { UserName = user.UserName, Authority = Request.RequestUri.Authority };
        //    List<GroupPermission> groupPermissions = new GroupPermissionHelper(this.user?.Id).Find().ToList();
        //    return Request.SuccessResult(new LoginResult
        //    {
        //        AccessToken = login.GetAccessToken(user.Password),
        //        AccountSerial = user.Id,
        //        UserName = user.UserName,
        //        FullName = user.FullName,
        //        GroupPermission_Id = user.GroupPermission_Id,
        //        ChucVu = groupPermissions.Where(p => p.Id == user.GroupPermission_Id).FirstOrDefault()?.Name,
        //        ImgUrl = user.Images,
        //        TokenType = "Bearer",
        //        ExpiresIn = (int)login.DateExpired.Subtract(DateTime.Now).TotalSeconds
        //    });
        //}
        /// <summary>
        /// Lấy danh sách người dùng
        /// </summary>
        /// <param name="iFilters">Tham số đầu vào</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<List<User>>))]
        public HttpResponseMessage Get([FromUri] AccountFilter iFilters)
        {
            List<User> vResult = new List<User>();
            vResult = helper.FindByFilter(iFilters);
            return Request.SuccessResult(vResult);
        }
        /// <summary>
        /// Lấy thông tin người dùng
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<User>))]
        public HttpResponseMessage Get(uint id)
        {
            var find = helper.FindById(id);
            if (find == null)
                throw new Exception("Không tìm thấy thông tin");
            return Request.SuccessResult(find);
        }
        /// <summary>
        /// Thêm mới thông tin người dùng
        /// </summary>
        /// <param name="value">Thông tin người dùng</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] User value)
        {
            return Request.SuccessResult(helper.AddUser(this.user, value, null));
        }
        /// <summary>
        /// Sửa thông tin người dùng
        /// </summary>
        /// <param name="id">ID đơn vị cần sửa</param>
        /// <param name="value">Thông tin người dùng mới</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [HttpPut]
        public HttpResponseMessage Put(string id, [FromBody] User value)
        {
            return Request.SuccessResult(helper.EditUser(this.user, value));
        }
        /// <summary>
        /// Cấp quyền người dùng
        /// </summary>
        /// <param name="id">ID đơn vị cần sửa</param>
        /// <param name="value">Thông tin người dùng mới</param>
        /// <returns></returns>
        //[ResponseType(typeof(APIResult<string>))]
        //[Route("Account/SetPermission/{id}")]
        //[HttpPost]
        //public HttpResponseMessage SetPermission(uint id, [FromBody] GroupPermission value)
        //{
        //    return Request.SuccessResult(helper.SetPermission(value, id));
        //}
        /// <summary>
        /// Xóa thông tin người dùng
        /// </summary>
        /// <param name="id">ID người dùng cần xóa</param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        public HttpResponseMessage Delete(uint id)
        {
            string data = helper.CheckDelete(id, this.user);

            return Request.SuccessResult(data);
        }
        /// <summary>
        /// Reset mật khẩu
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Account/ResetPass")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ResetPass([FromBody] ResetPassParam model)
        {
            //MailUser mailUser = new MailUserHelper(this.user).Find(p => p.Code == model.iCode).FirstOrDefault();
            //if (mailUser == null)
            //    throw new Exception("Thông tin yêu cầu reset mật khẩu không đúng.");

            //USers user1 = new AccountHelper(this.user).Find(p => p.Id == mailUser.UserId).FirstOrDefault();
            //if (user1 == null)
            //    throw new Exception("Tài khoản đã bị xóa hoặc không hoạt động, vui lòng liên hệ với quản trị viên.");

            //if ((DateTime.Now - mailUser.DateCreate).TotalDays > 3)
            //    throw new Exception("Yêu cầu reset mật khẩu đã hết hạn, xin vui lòng liên hệ quản trị viên.");

            //string value = new AccountHelper(this.user).ResetPass(user1.Id, model.iPassword);

            return Request.SuccessResult("OK");
        }
        /// <summary>
        /// Reset mật khẩu
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Account/ResetPassword")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ResetPassword(ResetPasswordParams value)
        {
            if (string.IsNullOrEmpty(value.iUserName))
                throw new Exception("Xin vui lòng nhập đầy đủ thông tin !");


            //USers user1 = new AccountHelper(new USers() { Id = "admin" }).Find(p => p.Id.ToLower() == value.iUserName.ToLower()).FirstOrDefault();
            //if (user1 == null)
            //    throw new Exception("Tài khoản không tồn tại trong hệ thống.");

            //if (string.IsNullOrEmpty(user1.Email))
            //    throw new Exception("Tài khoản này chưa có thông tin email xin vui lòng liên hệ với quản trị viên.");
            ////UpdateCacheUser

            //new MailUserHelper(new USers() { Id = "admin" }).SendMail(user1.Id, MailUserType.ResetPass);
            return Request.SuccessResult("OK");
        }
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Account/ChangePassword")]
        [HttpPost]
        public HttpResponseMessage ChangePassword([FromBody] ChanngePassObj value)
        {
            if (string.IsNullOrEmpty(value.oldPass) || string.IsNullOrEmpty(value.newPass) || string.IsNullOrEmpty(value.confirmPass))
                throw new Exception("Xin vui lòng nhập đầy đủ thông tin !");

            var checkPass = Security.EncryptPassword(value.oldPass.Trim());
            if (this.user.Password != checkPass)
                throw new Exception("Mật khẩu cũ không đúng !");
            if (value.newPass != value.confirmPass)
                throw new Exception("Mật khẩu xác nhận không đúng !");

            string encryptPass = Security.EncryptPassword(value.newPass);

            string data = helper.Update(this.user.Id, new List<UpdateDefinition<User>> {
                Builders<User>.Update.Set(p=>p.Password, encryptPass)
            }, null);

            //UpdateCacheUser

            return Request.SuccessResult(data);
        }
        [ResponseType(typeof(APIResult<string>))]
        [Route("Account/CheckLogin")]
        [HttpGet]
        public HttpResponseMessage CheckLogin()
        {
            return Request.SuccessResult("OK");
        }
        /// <summary>
        /// Thay đổi Avatar của người dùng
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(APIResult<string>))]
        [Route("Account/ChangeAvatar")]
        [HttpPost]
        public HttpResponseMessage ChangeAvatar()
        {
            uint vUser_id = 0;
            uint.TryParse(HttpContext.Current.Request.Form.Get("id"), out vUser_id);
            if (vUser_id != this.user.Id)
                throw new Exception("Bạn không có quyền thay đổi ảnh đại diện của người khác!");
            foreach (string fileName in HttpContext.Current.Request.Files)
            {
                var vFile = HttpContext.Current.Request.Files[fileName];
                string vFileName = vUser_id.ToString() + ".jpg";
                string vURL = "/UploadedFiles/Avatars/" + vFileName;
                helper.ChangeAvatar(vUser_id, vURL);
                string vFolder = HttpContext.Current.Server.MapPath("~/UploadedFiles/Avatars/");
                if (!Directory.Exists(vFolder))
                    Directory.CreateDirectory(vFolder);
                var fileSavePath = Path.Combine(vFolder, vFileName);
                vFile.SaveAs(fileSavePath);
                System.Drawing.Image image = System.Drawing.Image.FromFile(fileSavePath);
                int vMin = image.Width > image.Height ? image.Height : image.Width;
                var vNewImage = ((Bitmap)image).Clone(new Rectangle(0, 0, vMin, vMin), image.PixelFormat);
                var vImgNew = Utility.Images.ResizeImage(vNewImage, 100, 100);

                image.Dispose();
                vImgNew.Save(fileSavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return Request.SuccessResult("OK");
        }
    }
    public class ChanngePassObj
    {
        /// <summary>
        /// Mật khẩu cũ
        /// </summary>
        public string oldPass { get; set; }
        /// <summary>
        /// Mật khẩu mới
        /// </summary>
        public string newPass { get; set; }
        /// <summary>
        /// Xác nhận mật khẩu mối
        /// </summary>
        public string confirmPass { get; set; }
    }

    public class ResetPassParam
    {
        /// <summary>
        /// Mật khẩu mới
        /// </summary>
        public string iPassword { get; set; }
        /// <summary>
        /// Code reset
        /// </summary>
        public string iCode { get; set; }
    }
    public class ResetPasswordParams
    {
        /// <summary>
        /// Email
        /// </summary>
        public string iUserName { get; set; }
    }
}