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
    public class ClientAppAPIClient : APIClient
    {
        #region GetInstance

        private static Lazy<ClientAppAPIClient> _instance = new Lazy<ClientAppAPIClient>(() => new ClientAppAPIClient(WebConfig.SiteDomain_ClientAppService));

        /// <summary>
        /// 获取实例
        /// </summary>
        public static ClientAppAPIClient GetInstance
        {
            get 
            {
                return _instance.Value;
            }
        }

        private ClientAppAPIClient(string baseUrl)
            : base(baseUrl)
        { }

        #endregion
    }
}
