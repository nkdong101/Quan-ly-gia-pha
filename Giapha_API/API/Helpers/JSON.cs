using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Helpers
{
    public class JSON
    {
        /// <summary>
        /// convert object to jsonstring
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Stringify(object obj)
        {

            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        /// <summary>
        /// convert jsonstring to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}