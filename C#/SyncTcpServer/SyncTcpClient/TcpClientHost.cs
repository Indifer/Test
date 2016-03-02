using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncTcpClient
{
    /// <summary>
    /// Tcp客户端
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class TcpClientHost
    {
        private ITcpClientLog _iLog = null;
        private SessionHandle _seeesionHandle = null;
        private TcpClientHostConfig _tcpClientHostConfig;

        private TcpClientToken _clientToken = null;

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
        public TcpClientHost(TcpClientHostConfig tcpClientHostConfig, SessionHandle sessionHandle, ITcpClientLog tcpClientLog
            , Action<TcpClientToken> connectionToServerSuccessAction = null
            , Action<Exception> socketErrorAction = null)
        {
            _tcpClientHostConfig = tcpClientHostConfig;
            _iLog = tcpClientLog;
            _seeesionHandle = sessionHandle;
            _clientToken = new TcpClientToken(_seeesionHandle, _iLog);

            SocketErrorAction = socketErrorAction;
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
                if (!_clientToken.TcpClient.Connected)
                {
                    _clientToken.Connect(_tcpClientHostConfig.ServerHostname, _tcpClientHostConfig.ServerIPPoint);

                    if (ConnectionToServerSuccessAction != null)
                    {
                        ConnectionToServerSuccessAction(_clientToken);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (SocketErrorAction != null)
                {
                    Thread.Sleep(1000);
                    SocketErrorAction(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        public void StartReceive()
        {
            Action clientListener = () =>
            {
                while (true)
                {
                    string msg = null;

                    try
                    {
                        msg = _clientToken.Receive();
                        if (string.IsNullOrWhiteSpace(msg))
                        {
                            continue;
                        }
                    }
#if ANDROID
                    catch (System.IO.IOException ex)
                    {
                        continue;
                    }
#endif
                    catch (Exception ex)
                    {
                        return;
                    }

                    _clientToken.Session.ProcessReceive(msg, _clientToken);
                }
            };

            Thread _listenerThread = new Thread(() => { clientListener(); });

            _listenerThread.SetApartmentState(ApartmentState.STA);
            _listenerThread.IsBackground = true;
            _listenerThread.Start();
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
                _clientToken.Send(message, sendTimeout);
            }
            catch (Exception ex)
            {
                if (SocketErrorAction != null)
                {
                    SocketErrorAction(ex);
                }
                throw ex;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <returns></returns>
        public string Receive(int receiveTimeout = -1)
        {
            try
            {
                return _clientToken.Receive();
            }
            catch (Exception ex)
            {
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
