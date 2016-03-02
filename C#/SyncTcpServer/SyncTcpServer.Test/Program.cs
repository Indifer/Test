using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SyncTcpServer.Test
{
    class Program
    {
        public static int identity = 1;
        static TcpServerHost server;
        static void Main(string[] args)
        {
            IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
            server = new TcpServerHost(new TcpServerConfig(ipPoint:8181), new AccreditHandle(), new TcpServerLoger());

            server.AcceptSuccess = (x) =>
            {
                Console.WriteLine(string.Format("接入UserToken数：{0}", server.UserTokenCount()));
            };
            server.StartUp(IPAddress.Any);


            Console.ReadKey();
        }
    }

    public class TcpServerLoger : ITcpServerLog
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

    public class ShopSession : SessionHandle
    {

        public void ProcessReceive(string argument, UserToken token)
        {
            try
            {
                token.Send(argument);
            }
            catch { }
            //Console.WriteLine("UserToken{1}:ProcessReceive：{0}", argument, token.Identity);
        }

        public void ProcessSend(string argument, UserToken token)
        {
            Console.WriteLine("UserToken{1}:ProcessSend：{0}", argument, token.Identity);
        }

        //public override string Accredit(string argument)
        //{
        //    Console.WriteLine("Accredit：{0}", argument);
        //    return argument;
        //}

        public string Identity
        {
            get;
            set;
        }
    }

    public class AccreditHandle : ServerHandle
    {
        public SessionHandle Accredit(UserToken token)
        {
            var session = new ShopSession();
            session.Identity = (Program.identity++).ToString();
            return session;
        }
    }
}
