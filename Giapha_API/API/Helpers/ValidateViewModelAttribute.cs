using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace API.Controllers
{
    /// <summary>
    /// Xử lý lỗi model
    /// </summary>
    public class ValidateViewModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Action
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {








            //if (actionContext.ActionArguments.Any(kv => kv.Value == null))
            //{
            //    //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thông tin đầu vào không đúng");
            //    actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new JsonContent(new APIResult<string>(APIResultCode.ERROR, "Thông tin đầu vào không đúng"))
            //    };
            //}

            //if (actionContext.ModelState.IsValid == false)
            //{
            //    actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new JsonContent(new APIResult<ModelStateDictionary>(APIResultCode.ERROR, "Thông tin đầu vào không đúng", actionContext.ModelState))
            //    };
            //}
        }
    }
}