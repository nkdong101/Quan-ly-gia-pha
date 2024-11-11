using API.Models;
using API.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Utility;

namespace API.Controllers
{
    /// <summary>
    /// Exeption handling
    /// </summary>
    public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Ghi đè sự kiên exeption
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            SystemLog.WriteAPI(
                System.Reflection.MethodBase.GetCurrentMethod(), context.Request.RequestUri +
                "\n" + context.Exception.GetType().Name +
                "\n " + context.Exception.Message +
                "\n --------------------------------------");

            context.Response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(new APIResult<string>(APIResultCode.ERROR, "Lỗi! " + context.Exception.Message))
            };
        }
    }
}
