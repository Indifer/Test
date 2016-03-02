using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncTcpClient.Test
{
    public class Program
    {
        static void Main(string[] args)
        {

            for (var i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(() =>
                    {
                        Test();
                    }));

                thread.IsBackground = true;
                thread.Start();
                Thread.Sleep(50);
            }


            Console.ReadLine();

        }

        static void Test()
        {

            TcpClientHost Client;
            //终端服务器连接
            var serverSession = new ServerSession();
            TcpClientLog _log = new TcpClientLog();
            Client = new TcpClientHost(new TcpClientHostConfig("127.0.0.1", 8181)
                , serverSession
                , _log);
            Client.ConnectionToServerSuccessAction = x =>
            {
                if (!serverSession.Accredit(Client.ClientToken))
                {
                    _log.LogInfo(string.Format("未通过服务器端授权\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                {
                    Client.StartReceive();
                    _log.LogInfo(string.Format("通过服务器端授权\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
            };

            Client.SocketErrorAction = (ex) =>
            {

            };

            Client.ConnectionToServerSuccessAction = (T) =>
            {
                Action action = () =>
                    {
                        while (true)
                        {
                            T.Send(Guid.NewGuid().ToString());
                            Thread.Sleep(200);
                        }
                    };

                Thread thread = new Thread(new ThreadStart(action));
                thread.IsBackground = true;
                thread.Start();

                _log.LogInfo(Client.ClientToken.TcpClient.Client.LocalEndPoint.ToString());

            };

            Client.ConnectionToServer();
        }

    }


    /// <summary>
    /// 服务器Session
    /// </summary>
    public class ServerSession : SyncTcpClient.SessionHandle
    {
        public string Identity
        {
            get;
            set;
        }

        public void ProcessReceive(string argument, TcpClientToken token)
        {
            Console.WriteLine("ProcessSend：{0}", argument);
        }

        public void ProcessSend(string argument, TcpClientToken token)
        {
            Console.WriteLine("ProcessReceive：{0}", argument);
        }


        public bool Accredit(TcpClientToken token)
        {
            return true;
        }
    }

    public class TcpClientLog : SyncTcpClient.ITcpClientLog
    {
        public void LogDebug(string message, string fileName = null)
        {
            Console.WriteLine(message);
        }

        public void LogInfo(string message, string fileName = null)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message, Exception ex, string fileName = null)
        {
            Console.WriteLine(message);
        }
    }
}
