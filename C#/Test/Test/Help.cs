using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Rpc.HttpProtocol;

namespace Test
{
    public class Help
    {
        //Raven.Rpc.HttpProtocol.RpcHttpClient client = new RpcHttpClient("");
        public string Get()
        {
            return "1";
            //return client.Get<string>("user/get");
        }
    }
}
