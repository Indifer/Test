using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace ShoppingMall.Mobile.WebSite.Core.APIClient
{
    /// <summary>
    /// 
    /// </summary>
    public class APIClient2
    {
        private string baseUrl;
        [ThreadStatic]
        HttpClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        public APIClient2(string baseUrl)
        {
            this.baseUrl = baseUrl;
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType.json));
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="url">请求Url</param>
        /// <param name="mediaType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public virtual string Post(string url, IDictionary<string, string> data, string mediaType = MediaType.json, Encoding encoding = null)
        {
            MediaTypeFormatter mediaTypeFormatter = CreateMediaTypeFormatter(mediaType);

            string requestUrl = baseUrl + url;
            CreateUrlParams(data, ref requestUrl);

            using (HttpContent content = new ObjectContent<IDictionary<string, string>>(data, mediaTypeFormatter))
            {
                using (HttpResponseMessage response = client.PostAsync(requestUrl, content).Result)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
        }


        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private void CreateUrlParams(IDictionary<string, string> data, ref string baseUrl)
        {
            StringBuilder buffer = new StringBuilder();
            AddDefaultParameters(ref data);

            if (data != null)
            {
                int i = 0;
                foreach (string key in data.Keys)
                {
                    if (i == 0)
                    {
                        buffer.AppendFormat("{0}={1}", key, data[key]);
                        i++;
                    }
                    else
                    {
                        buffer.AppendFormat("&{0}={1}", key, data[key]);
                    }
                }
            }

            int index = baseUrl.IndexOf("?");
            if (index >= 0)
            {
                if (index < baseUrl.Length - 1)
                {
                    baseUrl += "&" + buffer.ToString();
                }
                else
                {
                    baseUrl += buffer.ToString();
                }
            }
            else
            {
                baseUrl += "?" + buffer.ToString();
            }
        }

        /// <summary>
        /// 创建HttpClient
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        private HttpClient CreateHttpClient(string mediaType = MediaType.json)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return client;
        }

        /// <summary>
        /// 创建MediaTypeFormatter
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        private MediaTypeFormatter CreateMediaTypeFormatter(string mediaType = MediaType.json)
        {
            MediaTypeFormatter mediaTypeFormatter = null;
            switch (mediaType)
            {
                case MediaType.form:
                    mediaTypeFormatter = new FormUrlEncodedMediaTypeFormatter();
                    break;
                case MediaType.xml:
                    mediaTypeFormatter = new XmlMediaTypeFormatter();
                    break;
                case MediaType.json:
                default:
                    mediaTypeFormatter = new JsonMediaTypeFormatter();
                    break;
            }

            return mediaTypeFormatter;
        }


        /// <summary>
        /// 添加默认参数
        /// </summary>
        /// <param name="data"></param>
        private void AddDefaultParameters(ref IDictionary<string, string> data)
        {
            //系统参数
            if (data == null)
            {
                data = new Dictionary<string, string>();
            }

            //if (System.Web.HttpContext.Current != null)
            //{
            //    var dp = HttpContextData.DefaultParameters;
            //    if (dp != null && dp.Count > 0)
            //    {
            //        foreach (var item in dp)
            //        {
            //            if (data.ContainsKey(item.Key)) continue;
            //            data.Add(item);
            //        }
            //    }
            //}
        }
    }
}
