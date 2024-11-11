using API.Models;
using API.Controllers;
using API.Helpers;
using MongoDB.Driver;
using MongoDBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using MongoDBAccess.Objects;
using Utility;

namespace API.Controllers

{
    public class CustomAuthorization : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            filterContext.Response = filterContext.Request.AuthResult();
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            var baseController = filterContext.ControllerContext.Controller as BaseController;
            System.Net.Http.Headers.AuthenticationHeaderValue auth = baseController.Request.Headers.Authorization;


            if (auth != null && auth.Scheme == "Bearer" && filterContext.Request.RequestUri.AbsolutePath.ToLower() != "/Account/Login".ToLower())
            {
                try
                {
                    string accessToken = auth.Parameter;
                    if (string.IsNullOrEmpty(accessToken))
                        throw new PermissionException("Không thấy thông tin token!");
                    var vLogin = new LoginUlti(1).FromAccessToken(accessToken);
                    var vUser = new MongoDBAccess.AccountHelper(1).FindById((uint)vLogin.Id);
                    if (vUser != null)
                    {
                        if (vLogin.GenericHash(vUser.Password) == vLogin.Hash)
                        {
                            baseController.user = vUser;
                            baseController.Request.Properties.Add("user", vUser);
                        }
                        else
                            throw new PermissionException("Mật khẩu đã bị thay đổi, vui lòng đăng nhập lại!");
                    }
                    else
                        throw new PermissionException("Vui lòng đăng nhập lại để thực hiện chức năng này!");
                    baseController.Token = accessToken;
                }
                catch (PermissionException ex)
                {
                    filterContext.Response = filterContext.Request.APIResult(new APIResult<string>(APIResultCode.Auth, ex.Message));
                }
                catch (Exception)
                {
                    base.OnAuthorization(filterContext);
                }
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}