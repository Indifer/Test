using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMall.Mobile.WebSite.Core
{
    public static class WebConfig
    {

        /// <summary>
        /// 用户端应用服务地址
        /// </summary>
        public static string SiteDomain_ClientAppService = ConfigurationManager.AppSettings["Service"].ToString();
    }
}
