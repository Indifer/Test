using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTcpServer
{
    /// <summary>
    /// Tcp服务器
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class TcpServerHost
    {
        #region filed

        private bool _isRun;
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
        /// 处理数据量
        /// </summary>
        public long totalBytes;

        /// <summary>
        /// 处理数据量
        /// </summary>
        public long sendCount;

        /// <summary>
        /// 处理数据量
        /// </summary>
        public long receiveCount;

        /// <summary>
        /// 授权成功Action
        /// </summary>
        public Action<UserToken> AcceptSuccess { get; set; }

        public Thread recordThreads;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tcpServerConfig"></param>
        /// <param name="accreditHandle"></param>
        /// <param name="iLog"></param>
        public TcpServerHost(TcpServerConfig tcpServerConfig, ServerHandle accreditHandle, ITcpServerLog iLog = null)
        {
            _isRun = false;

            _iLog = iLog;
            _accreditHandle = accreditHandle;
            //SessionFactory = sessionFactory;
            _tcpServerConfig = tcpServerConfig;
            _tokenPool = new UserTokenPool();

            var period = 60;

            recordThreads = new Thread(() => 
            {
                while (true)
                {
                    foreach (var _ut in _tokenPool.userTokens)
                    {
                        var _token = _ut.Value;
                        if (_token == null || _token.Socket == null || !_token.Socket.Connected
                            || _token.ActivityTime.AddHours(5) < DateTime.Now)
                        {
                            _tokenPool.RemoveUserToken(_token.Identity);
                            _token.Close();
                        }
                    }
                    //_tokenPool.RemoveUserToken(_token.Identity);
                    //_token.Close();

                    string msg = string.Format("bits per second: {0:N2}\r\nsend count per second: {1:N2}\r\nreceive count per second: {2:N2}\r\nsession count: {3}",
                       totalBytes / (double)period,
                       sendCount / (double)period,
                       receiveCount / (double)period, UserTokenCount());
                    totalBytes = 0;
                    sendCount = 0;
                    receiveCount = 0;
                    _iLog.LogInfo(msg, "record.txt");

                    Thread.Sleep(period * 1000);
                }
            });
            recordThreads.IsBackground = true;
            recordThreads.Start();

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

            Thread acceptThread = new Thread(() => AcceptAction(localSocket));
            acceptThread.IsBackground = true;
            acceptThread.Start();

            _isRun = true;

        }

        public void AcceptAction(Socket localSocket)
        {
            while (true)
            {
                try
                {
                    if (!_isRun)
                    {
                        return;
                    }

                    //clientConnected.Reset();
                    Socket clientSocket = localSocket.Accept();
                    _iLog.LogDebug("Accept:" + clientSocket.RemoteEndPoint.ToString());
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
        }

        /// <summary>
        /// 接入socket
        /// </summary>
        /// <param name="clientSocket"></param>
        private void AcceptSocket(Socket clientSocket)
        {
            UserToken token = new UserToken(clientSocket, _iLog, _tokenPool);
            token.SocketErrorAction = (_token, ex) =>
            {
                _tokenPool.RemoveUserToken(_token.Identity);
                _token.Close();
            };

            token.SendSuccessAction = x =>
            {
                System.Threading.Interlocked.Increment(ref sendCount);
                System.Threading.Interlocked.Add(ref totalBytes, x);
            };

            token.ReceiveSuccessAction = x =>
            {
                System.Threading.Interlocked.Increment(ref receiveCount);
                System.Threading.Interlocked.Add(ref totalBytes, x);
            };

            //_token防止函数闭包
            _accreditHandle.Accredit(token, AccreditCallback);
        }

        /// <summary>
        /// 授权回调
        /// </summary>
        /// <param name="token"></param>
        /// <param name="session"></param>
        public void AccreditCallback(UserToken token, SessionHandle session)
        {
            token.Session = session;
            AcceptEnd(token);
        }

        /// <summary>
        /// 授权完成
        /// </summary>
        /// <param name="token"></param>
        public void AcceptEnd(UserToken token)
        {
            if (token.Session == null)
            {
                if (_iLog != null)
                {
                    _iLog.LogError(string.Format("授权失败\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), null);
                }

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

            }
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
                        _tokenPool.RemoveUserToken(token.Identity);
                        token.Close();

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
            _isRun = false;
            try
            {
                recordThreads.Abort();
            }
            catch { }
            _tokenPool.Clear();
        }

    }
}
