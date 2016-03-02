using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTcpServer.Test
{
    class Program
    {
        public static int identity = 1;
        static TcpServerHost server;

        static void Main(string[] args)
        {
            //IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
            
            int ipPoint = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ipPoint"]);
            string ipBind = System.Configuration.ConfigurationManager.AppSettings["ipBind"];


            server = new TcpServerHost(new TcpServerConfig(ipPoint: ipPoint), new AccreditHandle(), TcpServerLoger.GetInstance);

            server.AcceptSuccess = (x) =>
            {
                Console.WriteLine(string.Format("接入UserToken数：{0}", server.UserTokenCount()));
            };

            IPAddress ipAddress;
            if (ipBind == "*")
            {
                ipAddress = IPAddress.Loopback;
            }
            else
            {
                ipAddress = IPAddress.Parse(ipBind);
            }

            server.StartUp(ipAddress);

            Console.ReadKey();

        }


        public class TcpServerLoger : AsyncTcpServer.ITcpServerLog
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            private static Dictionary<string, object> _obj = new Dictionary<string, object>();
            private TcpServerLoger()
            {
                var date = DateTime.Now.Date.AddDays(-7);
                string path = projectPath + "Log/";

                DeleteOldLog(path, date, 1);
            }

            /// <summary>
            /// 清除旧日志
            /// </summary>
            /// <param name="path"></param>
            /// <param name="folder"></param>
            private void DeleteOldLog(string path, DateTime date, int folderIndex)
            {
                int folder;
                switch (folderIndex)
                {
                    case 1:
                        folder = date.Year;
                        break;
                    case 2:
                        folder = date.Month;
                        break;
                    case 3:
                        folder = date.Day;
                        break;
                    default:
                        return;
                }

                var dire = new DirectoryInfo(path);
                if (dire == null || !dire.Exists)
                {
                    return;
                }

                foreach (var d in dire.GetDirectories())
                {
                    int temp;
                    if (int.TryParse(d.Name, out temp))
                    {
                        if (temp < folder)
                        {
                            Directory.Delete(d.FullName, true);
                        }
                        else if (temp == folder)
                        {
                            DeleteOldLog(d.FullName, date, folderIndex + 1);
                        }
                    }
                }
            }

            private static TcpServerLoger _instance = new TcpServerLoger();
            /// <summary>
            /// 获取实例
            /// </summary>
            public static TcpServerLoger GetInstance
            {
                get
                {
                    return _instance;
                }
            }

            public void LogDebug(string message, string fileName = null)
            {
                //Console.WriteLine("-----------------------------------");
                //Console.WriteLine(message);

                if (message.IndexOf("$[Heartbeat]$", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    return;
                }

                fileName = fileName ?? "debug.txt";
                SaveLog(fileName, message);

            }

            public void LogInfo(string message, string fileName = null)
            {

                Console.WriteLine("-----------------------------------");
                Console.WriteLine(message);

                fileName = fileName ?? "info.txt";
                SaveLog(fileName, message);

            }

            public void LogError(string message, Exception ex, string fileName = null)
            {

                Console.WriteLine("-----------------------------------");
                Console.WriteLine(message);
                if (ex != null)
                {
                    Console.WriteLine(ex.StackTrace);
                }

                fileName = fileName ?? "error.txt";
                if (ex is System.Net.Sockets.SocketException)
                {
                    fileName = "socket.txt";
                }
                else if (ex is System.IO.IOException)
                {
                    fileName = "io.txt";
                }

                if (ex != null)
                {
                    message += "\r\n" + ex.StackTrace;
                    if (ex.InnerException != null)
                    {
                        message += "\r\n内部错误：" + ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace;
                    }
                }

                SaveLog(fileName, message);

            }

            private void SaveLog(string fileName, string message)
            {
                DateTime nowTime = DateTime.Now;
                message = "\r\n" + nowTime.ToString("yyyy-MM-dd HH:mm:ss") + "-----------------------------------\r\n" + message;

                string path = projectPath + "Log\\" + string.Format("{0}\\{1}\\{2}\\", nowTime.Year, nowTime.Month, nowTime.Day);// + DateTime.Now.ToString("yyyy/MM/dd/");

                path = System.IO.Path.GetFullPath(path);

                Action<object> act = (o) =>
                {
                    if (!_obj.ContainsKey(fileName))
                    {
                        _obj.Add(fileName, new object());
                    }

                    lock (_obj[fileName])
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        path = path + fileName;
                        try
                        {
                            File.AppendAllText(path, message);
                        }
                        catch { }
                    }
                };

                if (!System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(act)))
                {
                    act(null);
                }
            }
        }


        public class ShopSession : SessionHandle
        {

            public void ProcessReceive(string argument, UserToken token)
            {
                //Console.WriteLine("UserToken{0}:ProcessReceive：{1}", token.Identity, argument);
            }

            public string Identity
            {
                get;
                set;
            }
        }

        public class AccreditHandle : ServerHandle
        {
            public void Accredit(UserToken token, Action<UserToken, SessionHandle> accreditSession)
            {
                var session = new ShopSession();
                session.Identity = (Program.identity++).ToString();
                accreditSession(token, session);
            }
        }

    }

}
