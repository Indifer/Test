using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace SSTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new AppServer();
            appServer.NewSessionConnected += appServer_NewSessionConnected;
            appServer.NewRequestReceived += appServer_NewRequestReceived;

            //Setup the appServer
            if (!appServer.Setup(2012)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        static void appServer_NewRequestReceived(AppSession session, SuperSocket.SocketBase.Protocol.StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Body);
            Console.WriteLine(requestInfo.Key);
            Console.WriteLine(requestInfo.Parameters);

        }

        static void appServer_NewSessionConnected(AppSession session)
        {
            Console.WriteLine(session.SessionID);
            session.Send("Welcome");
        }


        ///// <summary>
        ///// 使用加密服务提供程序实现加密生成随机数
        ///// </summary>
        ///// <param name="length"></param>
        ///// <returns>16进制格式字符串</returns>
        //public static string CreateMachineKey(int length)
        //{
        //    // 要返回的字符格式为16进制,byte最大值255
        //    // 需要2个16进制数保存1个byte,因此除2
        //    byte[] random = new byte[length / 2];

        //    // 使用加密服务提供程序 (CSP) 提供的实现来实现加密随机数生成器 (RNG)
        //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        //    // 用经过加密的强随机值序列填充字节数组
        //    rng.GetBytes(random);

        //    StringBuilder machineKey = new StringBuilder(length);
        //    for (int i = 0; i < random.Length; i++)
        //    {
        //        machineKey.Append(string.Format("{0:X2}", random[i]));
        //    }
        //    return machineKey.ToString();
        //}
    }
}
