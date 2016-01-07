using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ConsoleApp
{
    public static class HttpRequestHelper
    {
        public static string GetResponse(string urlPath, IDictionary<string, string> parameters, int? timeout = null, string userAgent = "", Encoding requestEncoding = null, CookieCollection cookies = null, Action<string> action = null)
        {
            if (string.IsNullOrEmpty(urlPath))
                throw new ArgumentNullException("urlPath");
            if (requestEncoding == null)
                requestEncoding = Encoding.UTF8;

            string res = "";
            string url = urlPath;
            HttpWebRequest request = null;

            request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = userAgent;

            if (timeout.HasValue)
                request.Timeout = timeout.Value;

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            StringBuilder buffer = new StringBuilder();
            int i = 0;
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }
            var bufferStr = buffer.ToString();

            ////记录请求日志
            //Loger.GetInstance.LogInfo("----------------------------");
            //Loger.GetInstance.LogInfo(urlPath);
            //Loger.GetInstance.LogInfo(bufferStr);
            //Loger.GetInstance.LogInfo("----------------------------");
            if (action != null)
            {
                action(url + "?" + bufferStr);
            }
            byte[] data = requestEncoding.GetBytes(bufferStr);
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        res = sr.ReadToEnd();
                        sr.Close();
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                //res = "{\"m\":-1,\"_e\":\"调用API时出错:" + ex.Message + "\"}";
                throw ex;
            }
            ////记录返回日志
            //Loger.GetInstance.LogInfo("----------------------------");
            //Loger.GetInstance.LogInfo(urlPath);
            //Loger.GetInstance.LogInfo(res);
            //Loger.GetInstance.LogInfo("----------------------------");
            return res;
        }

    }
}
