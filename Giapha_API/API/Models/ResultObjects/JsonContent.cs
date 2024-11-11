using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// JSON content
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public JsonContent(object data) : base(JsonConvert.SerializeObject(data, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }), Encoding.UTF8, "application/json")
        {
        }
    }
}