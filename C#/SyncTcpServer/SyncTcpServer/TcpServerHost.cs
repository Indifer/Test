using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncTcpServer
{
    /// <summary>
    /// Tcp服务器
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class TcpServerHost
    {
        #region filed

        private ITcpServerLog _iLog = null;
        private UserTokenPool _tokenPool = null;
        //private Func<string, SessionHandle> SessionFactory = null;
        private ServerHandle _accreditHandle = null;
        private TcpServerConfig _tcpServerConfig = null;
        //private Func<string> AccreditFailSendMessage = null;
        //internal ManualResetEvent clientConnected = new ManualResetEvent(false);
        //internal ConcurrentDictionary<int, AutoResetEvent> autoResetEventDict = new ConcurrentDictionary<int, AutoResetEvent>();

        #endregion

        /// <summary>
        /// 授权成功Action
        /// </summary>
        public Action<UserToken> AcceptSuccess = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tcpServerConfig"></param>
        /// <param name="accreditHandle"></param>
        /// <param name="iLog"></param>
        public TcpServerHost(TcpServerConfig tcpServerConfig, ServerHandle accreditHandle, ITcpServerLog iLog = null)
        {
            _iLog = iLog;
            _accreditHandle = accreditHandle;
            //SessionFactory = sessionFactory;
            _tcpServerConfig = tcpServerConfig;
            _tokenPool = new UserTokenPool();
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        public void StartUp(IPAddress ipAddress)
        {
            //获取监听者端口
            IPEndPoint serverPoint = new IPEndPoint(ipAddress, _tcpServerConfig.IPPoint);

            //获取监听传入的socket
            Socket localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Socket与本地端点关联
            localSocket.Bind(serverPoint);

            //TcpListener listener = new TcpListener(IPAddress.Any, _iPPoint);
            //listener.Start();

            if (_iLog != null)
            {
                _iLog.LogInfo(string.Format("服务已启动...\n\rlocal endpoint = {0}:{1}", serverPoint.Address.ToString(), serverPoint.Port));
            }

            localSocket.Listen(100);

            Action acceptAction = new Action(() =>
            {
                while (true)
                {
                    try
                    {
                        //clientConnected.Reset();
                        Socket clientSocket = localSocket.Accept();
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                AcceptSocket(clientSocket);
                            }
                            catch (Exception ex)
                            {
                                _iLog.LogError("Accept:" + ex.Message, ex);
                            }
                        });

                        //localSocket.BeginAccept(new AsyncCallback(AcceptSocketCallback), localSocket);
                        //listener.BeginAcceptSocket(new AsyncCallback(AcceptSocketCallback), listener);

                        //var autoReset = new AutoResetEvent(false);
                        //autoResetEventDict.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, autoReset, (x,y) => autoReset);
                        //clientConnected.WaitOne();
                    }
                    catch (Exception ex)
                    {
                        if (_iLog != null)
                        {
                            _iLog.LogError(string.Format("远程客户端建立socket失败，Message:{0}...", ex.Message), ex);
                        }
                    }
                }
            });

            Thread acceptThread = new Thread(() => acceptAction());
            acceptThread.IsBackground = true;
            acceptThread.Start();

        }

        ///// <summary>
        ///// AcceptSocket回调
        ///// </summary>
        ///// <param name="ar"></param>
        //private void AcceptSocketCallback(IAsyncResult ar)
        //{
        //    Thread thread = new Thread(() => {

        //        //TcpListener listener = (TcpListener)ar.AsyncState;
        //        //Socket socket = listener.EndAcceptSocket(ar);
        //        //localsocket
        //        Socket localSocket = (Socket)ar.AsyncState;
        //        //接入socket
        //        Socket socket = localSocket.EndAccept(ar);
        //        UserToken token = new UserToken(socket);

        //        string receive = null;
        //        try
        //        {
        //            receive = token.Receive();
        //        }
        //        //异常
        //        catch
        //        {
        //            token.Close();
        //            clientConnected.Set();
        //            return;
        //        }
        //        token.Session = SessionFactory(receive);
        //        string identity = token.Session.Accredit(receive);

        //        if (string.IsNullOrWhiteSpace(identity))
        //        {
        //            token.Close();
        //            token = null;
        //        }
        //        else
        //        {
        //            token.Identity = identity;
        //            _tokenPool.AddUserToken(token);
        //            try
        //            {
        //                token.Listener();
        //            }
        //            catch 
        //            {
        //                token.Close();
        //            }
        //        }
        //    });
        //    thread.IsBackground = true;
        //    thread = UserThreadPool.AddThread("[ServerAcceptSocket]", thread);
        //    thread.Start();

        //    //var autoReset = new AutoResetEvent(false);
        //    //autoResetEventDict.TryGetValue(Thread.CurrentThread.ManagedThreadId, autoReset, (x, y) => autoReset);
        //    clientConnected.Set();
        //}

        /// <summary>
        /// 接入socket
        /// </summary>
        /// <param name="clientSocket"></param>
        private void AcceptSocket(Socket clientSocket)
        {
            //TcpListener listener = (TcpListener)ar.AsyncState;
            //Socket socket = listener.EndAcceptSocket(ar);
            //clientSocket.ReceiveTimeout = _tcpServerConfig.AccreditReceiveTimeout;
            UserToken token = new UserToken(clientSocket, _iLog, _tokenPool);

            token.Session = _accreditHandle.Accredit(token);

            if (token.Session == null)
            {
                if (_iLog != null)
                {
                    _iLog.LogError(string.Format("授权失败\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), null);
                }

                //if (_accreditHandle != null)
                //{
                //    try
                //    {
                //        token.Send(_accreditHandle.AccreditFailSendMessage());
                //    }
                //    catch { }
                //}
                token.Close();
                return;
            }
            else
            {
                if (_iLog != null)
                {
                    _iLog.LogDebug(string.Format("授权成功\r\nIdentity:{0}\r\n{1}", token.Session.Identity, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }

            string identity = token.Session.Identity;

            if (string.IsNullOrWhiteSpace(identity))
            {
                token.Close();
                token = null;
            }
            else
            {
                token.Identity = identity;
                _tokenPool.AddUserToken(token);

                if (AcceptSuccess != null)
                {
                    AcceptSuccess(token);
                }

                //token.Socket.ReceiveTimeout = -1;

                Action clientListener = () =>
                {
                    while (true)
                    {
                        string msg = null;

                        try
                        {
                            msg = token.Receive();
                            if (string.IsNullOrWhiteSpace(msg))
                            {
                                continue;
                            }
                        }
#if ANDROID
                        catch (System.IO.IOException ex)
                        {
                        }
#endif
                        catch (Exception ex)
                        {
                            _tokenPool.RemoveUserToken(token.Identity);
                            token.Close();
                        }
                        //异常
                        if (msg == null)
                        {
                            return;
                        }
                        token.Session.ProcessReceive(msg, token);
                    }
                };

                Thread _listenerThread = new Thread(() => { clientListener(); });
                //thread = UserThreadPool.AddThread(Identity, thread);
                _listenerThread.IsBackground = true;
                _listenerThread.Start();
                
            }

            //var autoReset = new AutoResetEvent(false);
            //autoResetEventDict.TryGetValue(Thread.CurrentThread.ManagedThreadId, autoReset, (x, y) => autoReset);
            //clientConnected.Set(); 
        }

        /// <summary>
        /// 发送信息到全部token
        /// </summary>
        /// <param name="message"></param>
        /// <param name="filter"></param>
        public void SendToAllUserToken(string message, Func<UserToken, bool> filter = null)
        {
            foreach (var token in _tokenPool.userTokens.Values)
            {
                if (filter == null || filter(token))
                {
                    try
                    {
                        token.Send(message);
                    }
                    catch (Exception ex)
                    {
#if ANDROID
#else
                        _tokenPool.RemoveUserToken(token.Identity);
                        token.Close();
#endif

                        _iLog.LogError(ex.Message, ex);
                    }
                }
            }

        }

        /// <summary>
        /// 获取UserToken数
        /// </summary>
        /// <returns></returns>
        public int UserTokenCount()
        {
            return _tokenPool.Count();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _tokenPool.Clear();
        }

    }
}
