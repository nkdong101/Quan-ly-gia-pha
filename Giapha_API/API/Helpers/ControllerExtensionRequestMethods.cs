using API.Models;
using API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Utility;

namespace API.Controllers
{

    public static class ControllerExtensionRequestMethods
    {

        /// <summary>
        /// Return Response By APIResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResponseMessage APIResult<T>(this HttpRequestMessage obj, APIResult<T> data)
        {
            return obj.CreateResponse(System.Net.HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Request thành công
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResponseMessage SuccessResult<T>(this HttpRequestMessage obj, T data)
        {
            return obj.CreateResponse(System.Net.HttpStatusCode.OK, new APIResult<T>(data));
        }

        /// <summary>
        /// Request for lỗi chưa đăng nhập
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static HttpResponseMessage AuthResult(this HttpRequestMessage obj)
        {
            SystemLog.WriteAPI(System.Reflection.MethodBase.GetCurrentMethod(), obj.RequestUri.AbsoluteUri + "\n");
            return obj.CreateResponse(System.Net.HttpStatusCode.OK, new APIResult<string>(APIResultCode.Auth, "Bạn cần đăng nhập lại để sử dụng tính năng này!"));
        }


        /// <summary>
        /// Request cho lỗi
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpResponseMessage FAILResult(this HttpRequestMessage obj, string message)
        {
            SystemLog.WriteAPI(System.Reflection.MethodBase.GetCurrentMethod(), obj.RequestUri.AbsoluteUri + "\n");
            return obj.CreateResponse(System.Net.HttpStatusCode.OK, new APIResult<string>(APIResultCode.ERROR, message));
        }

        /// <summary>
        /// Request cho lỗi kèm data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResponseMessage FAILResult<T>(this HttpRequestMessage obj, string message, T data)
        {
            SystemLog.WriteAPI(System.Reflection.MethodBase.GetCurrentMethod(), obj.RequestUri.AbsoluteUri + "\n" + JSON.Stringify(data));
            return obj.CreateResponse(System.Net.HttpStatusCode.OK, new APIResult<T>(APIResultCode.ERROR, message, data));
        }

      


    }
}