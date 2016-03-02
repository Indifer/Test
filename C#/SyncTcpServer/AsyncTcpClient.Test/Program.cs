using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTcpClient.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 1; i++)
            {
                //Thread thread = new Thread(new ThreadStart(() =>
                //{
                //    Test();
                //}));

                System.Threading.ThreadPool.QueueUserWorkItem((x) =>
                {
                    Test();
                });

                //thread.IsBackground = true;
                //thread.Start();
                //Thread.Sleep(50);
            }


            Console.ReadLine();
        }


        static void Test()
        {

            int ipPoint = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ipPoint"]);
            string ipBind = System.Configuration.ConfigurationManager.AppSettings["ipBind"];

            TcpClientHost Client;
            //终端服务器连接
            var serverSession = new ServerSession();

            Client = new TcpClientHost(new TcpClientConfig(ipBind, ipPoint)
                , serverSession
                , TcpClientLog.GetInstance);
            Client.ConnectionToServerSuccessAction = x =>
            {
                serverSession.Accredit(Client.ClientToken, (result) =>
                {
                    if (result)
                    {
                        TcpClientLog.GetInstance.LogInfo(string.Format("未通过服务器端授权\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    else
                    {
                        TcpClientLog.GetInstance.LogInfo(string.Format("通过服务器端授权\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                });
            };

            Client.SocketErrorAction = (ex) =>
            {
                TcpClientLog.GetInstance.LogError(ex.Message, ex, "socket.txt");
            };



            Client.ConnectionToServerSuccessAction = (T) =>
            {
                Action action = () =>
                {
                    while (true)
                    {
                        try
                        {
                            int seed = 50000;
                            Task[] tasks = new Task[seed];

                            Stopwatch sw = new Stopwatch();
                            sw.Restart();
                            for (var i = 0; i < seed; i++)
                            {
                                tasks[i] = T.SendAsync("{" + 1 + "}");
                                Thread.SpinWait(1);
                            }

                            Task.WaitAll(tasks);
                            sw.Stop();
                            Console.WriteLine("Count:{0},Milliseconds:{1}", seed, sw.ElapsedMilliseconds);
                            //T.SendAsync("{" + Guid.NewGuid().ToString() + DateTime.Now.ToShortDateString() + "}").Wait();
                        }
                        catch (Exception ex)
                        {

                        }

                        Thread.Sleep(5000);
                    }
                };

                Thread thread = new Thread(new ThreadStart(action));
                thread.IsBackground = true;
                thread.Start();

                TcpClientLog.GetInstance.LogInfo(Client.ClientToken.TcpClient.Client.LocalEndPoint.ToString());

            };

            Client.ConnectionToServer();
        }
    }



    /// <summary>
    /// 服务器Session
    /// </summary>
    public class ServerSession : AsyncTcpClient.SessionHandle
    {
        public string Identity
        {
            get;
            set;
        }

        public void ProcessReceive(string argument, TcpClientToken token)
        {
            //Console.WriteLine("ProcessSend：{0}", argument);
        }


        public void Accredit(TcpClientToken token, Action<bool> accreditEnd)
        {
            accreditEnd(true);
        }
    }

    public class TcpClientLog : AsyncTcpClient.ITcpClientLog
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        private static Dictionary<string, object> _obj = new Dictionary<string, object>();
        private TcpClientLog()
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

        private static TcpClientLog _instance = new TcpClientLog();
        /// <summary>
        /// 获取实例
        /// </summary>
        public static TcpClientLog GetInstance
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

            //if (message.IndexOf("$[Heartbeat]$", StringComparison.InvariantCultureIgnoreCase) >= 0)
            //{
            //    return;
            //}

            //fileName = fileName ?? "debug.txt";
            //SaveLog(fileName, message);

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


}
