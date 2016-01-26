using MsgPack;
using MsgPack.Rpc.Client;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientConApp
{
    public class User
    {
        public string Name;
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var target = CreateClient())
            {
                //var result1 = target.Call("Hello:Methods:0", null); /*if in the server the property UseFullMethodName is true, or not defined as false (default is true) you have to call the method on the server using *methodname:class:version**/
                var result2 = target.Call("Hello"); //Parameter is null
                var result3 = target.Call("HelloParam", "Mariane");//Method with parameter

                var user = MessagePackSerializer.Get<User>().UnpackSingleObject(result3.AsBinary());
                
                Console.WriteLine(result2);
                Console.WriteLine(result3);
            }
        }

        public static RpcClient CreateClient()
        {
            return new RpcClient(new IPEndPoint(IPAddress.Loopback, 8089), new RpcClientConfiguration() { PreferIPv4 = true });
        }

    }
}
