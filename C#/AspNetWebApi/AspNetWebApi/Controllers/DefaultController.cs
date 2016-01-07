using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace AspNetWebApi.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpPost]
        [ApiFilter]
        public string Add(string a, string b)
        {

            //NameValueCollection data = ReadAsFormDataAsync();

            //string val = "";
            //foreach (var k in data.AllKeys)
            //{
            //    val += string.Format("{0}:{1},", k, data[k]);
            //}            

            //return val;
            Thread.Sleep(10);
            return a + b;
        }

        public string Add2()
        {
            JObject data = ReadAsJObjectAsync();

            string val = "";
            foreach (var k in data)
            {
                val += string.Format("{0}:{1},", k.Key, k.Value);
            }

            return val;
        }

        [HttpPost]
        public string Add3(int? a , string b = null)
        {
            return string.Format("Add:{0},{1}", a, b);
        }


        private JObject ReadAsJObjectAsync()
        {
            var data = Request.Content.ReadAsAsync<JObject>().Result;
            return data;
        }

        private NameValueCollection ReadAsFormDataAsync()
        {
            var data = Request.Content.ReadAsFormDataAsync().Result;
            return data;
        }
    }


    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
