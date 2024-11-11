using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class GetDataAPIModel
    {
        public string url { get; set; }
        public object body { get; set; }
        public Dictionary<string, string> headers { get; set; }


    }
    public class GetDataAPI
    {

        public static T GET<T>(GetDataAPIModel model)
        {

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(model.url);
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
                httpWebRequest.Method = "GET";
                if (model.headers != null)
                {
                    model.headers.ToList().ForEach(p =>
                    {
                        httpWebRequest.Headers.Add(p.Key, p.Value);
                    });
                }


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string responseText = GetResponseStream(httpResponse);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    T result = JsonConvert.DeserializeObject<T>(responseText);
                    return result;
                }
                else
                {
                    throw new Exception(responseText);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static T POST<T>(GetDataAPIModel model)
        {

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(model.url);
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
                httpWebRequest.Method = "POST";
                if (model.headers != null)
                {
                    model.headers.ToList().ForEach(p =>
                    {
                        httpWebRequest.Headers.Add(p.Key, p.Value);
                    });
                }

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    if (model.body != null)
                        streamWriter.Write(JsonConvert.SerializeObject(model.body));
                    else
                        streamWriter.Write(JsonConvert.SerializeObject(new { }));
                }


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string responseText = GetResponseStream(httpResponse);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    T result = JsonConvert.DeserializeObject<T>(responseText);
                    return result;
                }
                else
                {
                    throw new Exception(responseText);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string GetResponseStream(HttpWebResponse response)
        {
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
    }
}
