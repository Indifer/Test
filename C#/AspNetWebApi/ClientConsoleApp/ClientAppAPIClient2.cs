using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMall.Mobile.WebSite.Core.APIClient
{
    /// <summary>
    /// 用户端应用服务调用客户端
    /// </summary>
    public class ClientAppAPIClient2 : APIClient2
    {
        #region GetInstance

        private static Lazy<ClientAppAPIClient2> _instance = new Lazy<ClientAppAPIClient2>(() => new ClientAppAPIClient2(WebConfig.SiteDomain_ClientAppService));

        /// <summary>
        /// 获取实例
        /// </summary>
        public static ClientAppAPIClient2 GetInstance
        {
            get 
            {
                return _instance.Value;
            }
        }

        private ClientAppAPIClient2(string baseUrl)
            : base(baseUrl)
        { }

        #endregion
    }
}
