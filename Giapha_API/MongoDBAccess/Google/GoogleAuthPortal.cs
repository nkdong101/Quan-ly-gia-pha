using MongoDB.Driver;
using MongoDBAccess.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace MongoDBAccess.Google
{
    public class GoogleAuthPortal
    {
        public static string client_id => "924322044127-ljktmq9u9bmj4k6cvj8g76ch273te3s4.apps.googleusercontent.com";
        public static string client_secret => "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";
        public static string auth_base => "https://oauth2.googleapis.com/token";
        public static string user_base => "https://www.googleapis.com/oauth2/v3/userinfo";


        public static LoginSocial GetUserInfo(string access_token)
        {

            LoginSocial LoginSocial = GetDataAPI.GET<LoginSocial>(new GetDataAPIModel
            {
                url = user_base,
                headers = new Dictionary<string, string>
                    {
                        {"Authorization",$"Bearer {access_token}" }
                    }
            });

            return LoginSocial;

        }
        /// <summary>
        /// Hàm lấy code từ web call lên khi login google thành công
        /// Giải mã code lấy access_token, refresh_token để trả cho client
        /// </summary>
        /// <param name="code"></param>
        /// <param name="redirect_uri"></param>
        public static LoginSocial Login(string code, string redirect_uri)
        {
            try
            {
                var uri = auth_base;
                var para = new List<string>
                {
                    $"client_id={client_id}",
                    $"client_secret={client_secret}",
                    $"redirect_uri={redirect_uri}",
                    $"code={code}",
                    $"grant_type=authorization_code",
                };
                uri += $"?{string.Join("&", para)}";

                TokenInfo tokenInfo = GetDataAPI.POST<TokenInfo>(new GetDataAPIModel
                {
                    url = uri
                });
                if (tokenInfo == null)
                    throw new Exception("Failed");

                LoginSocial loginSocial = GetUserInfo(tokenInfo.access_token);
                if (loginSocial == null)
                    throw new Exception("Failed");

                AccountHelper accountHelper = new AccountHelper(1);
                Models.User user = accountHelper.Find(p => p.Email.ToLower() == loginSocial.email.ToLower()).FirstOrDefault();
                if (user == null)
                {
                    throw new InputException($"Bạn đang đăng nhập với email <b>{loginSocial.email.ToLower()}</b> mà chưa được đăng ký trên hệ thống. Vui lòng liên hệ quản trị viên!");
                }

                loginSocial.access_token = tokenInfo.access_token;
                UserLoginSocialHelper userLoginSocialHelper = new UserLoginSocialHelper(1);
                Models.UserLoginSocial userLoginSocial = userLoginSocialHelper.Find(p => p.email == loginSocial.email.ToLower()).FirstOrDefault();
                if (userLoginSocial == null)
                {
                    userLoginSocial = new Models.UserLoginSocial
                    {
                        Id = user.Id,
                        email = loginSocial.email.ToLower(),
                        AccessTokens = new List<string>
                        {
                            tokenInfo.access_token
                        },
                        Info = loginSocial,
                        refresh_token = tokenInfo.refresh_token,
                        LastLogin = DateTime.Now,
                    };
                    userLoginSocialHelper.Create(userLoginSocial, null);
                }
                else
                {


                    userLoginSocialHelper.Update(userLoginSocial.Id, u => u
                        .Set(p => p.refresh_token, tokenInfo.refresh_token)
                        .Set(p => p.Info, loginSocial)
                        .Set(p => p.LastLogin, DateTime.Now)
                        .Push(p => p.AccessTokens, tokenInfo.access_token),
                        null
                    );
                }
                return loginSocial;
            }
            catch (InputException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Đăng nhập không thành công, vui lòng đăng nhập lại");
            }

        }
    }
}
