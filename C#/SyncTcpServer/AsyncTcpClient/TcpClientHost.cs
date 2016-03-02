using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTcpClient
{
    /// <summary>
    /// Tcp客户端
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class TcpClientHost
    {
        private ITcpClientLog _iLog = null;
        private SessionHandle _seeesionHandle = null;
        private TcpClientConfig _tcpClientHostConfig;

        private TcpClientToken _clientToken = null;

        private object _lockObj = new object();

        /// <summary>
        /// 连接服务器成功接口
        /// </summary>
        public Action<TcpClientToken> ConnectionToServerSuccessAction;

        /// <summary>
        /// Socket异常
        /// </summary>
        public Action<Exception> SocketErrorAction;

        /// <summary>
        /// 
        /// </summary>
        public TcpClientToken ClientToken
        {
            get { return _clientToken; }
            set { _clientToken = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tcpClientHostConfig"></param>
        /// <param name="sessionHandle"></param>
        public TcpClientHost(TcpClientConfig tcpClientHostConfig, SessionHandle sessionHandle, ITcpClientLog tcpClientLog
            , Action<TcpClientToken> connectionToServerSuccessAction = null
            , Action<Exception> socketErrorAction = null)
        {
            _tcpClientHostConfig = tcpClientHostConfig;
            _iLog = tcpClientLog;
            _seeesionHandle = sessionHandle;
            _clientToken = new TcpClientToken(_seeesionHandle, _iLog);

            SocketErrorAction = socketErrorAction;
            _clientToken.SocketErrorAction = socketErrorAction;
            ConnectionToServerSuccessAction = connectionToServerSuccessAction;
        }

        /// <summary>
        /// 连接至服务器
        /// </summary>
        /// <returns></returns>
        public bool ConnectionToServer()
        {
            try
            {
                if (_clientToken == null || _clientToken.TcpClient == null)
                    return false;
                if (_clientToken.TcpClient.Connected)
                {
                    return true;
                }
                lock (_lockObj)
                {
                    if (!_clientToken.TcpClient.Connected)  
                    {
                        _clientToken.Connect(_tcpClientHostConfig.ServerHostname, _tcpClientHostConfig.ServerIPPoint);

                        if (ConnectionToServerSuccessAction != null)
                        {
                            ConnectionToServerSuccessAction(_clientToken);
                        }

                        _clientToken.BeginReceive();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (SocketErrorAction != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(1000);
                        SocketErrorAction(ex);
                    }).LogExceptions((e) => { _iLog.LogError(e.Message, e, "task.txt"); });
                }
                return false;
            }
        }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            if (_clientToken != null)// && _clientToken.TcpClient != null && _clientToken.TcpClient.Client != null)
            {
                return _clientToken.IsConnected();
            }
            else return false;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sendTimeout">发送超时值（以毫秒为单位）。默认值为 0。</param>
        public void Send(string message, int sendTimeout = 0)
        {
            try
            {
#if ANDROID
                sendTimeout = 10000;
#endif
                _clientToken.Send(message, sendTimeout);

            }
            catch (Exception ex)
            {
                //if (ex is SocketException)
                //{
                //    if((ex as SocketException).SocketErrorCode == SocketError.NotConnected)
                //    {
                //        throw ex;
                //    }
                //}
                if (SocketErrorAction != null)
                {
                    SocketErrorAction(ex);
                }
                throw ex;
            }
        }


        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (_clientToken != null)
            {
                _clientToken.Close();
            }

        }
    }
}
