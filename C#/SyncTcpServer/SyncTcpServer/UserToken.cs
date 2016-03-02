using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SyncTcpServer
{
    public delegate void SocketExceptionHandler(string identity, object obj);
    /// <summary>
    /// 用户token
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class UserToken : IDisposable
    {
        private ITcpServerLog _iLog = null;
        private UserTokenPool _tokenPool = null;
        private Socket _socket;
        private object _lockObj = new object();
#if ANDROID
        private TcpBinaryReader _reader;
#else
        private BinaryReader _reader;
#endif
        private BinaryWriter _writer;
        private SessionHandle _session { get; set; }
        //private Thread _listenerThread = null;
        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }

        public SessionHandle Session
        {
            get { return _session; }
            set
            {
                _session = value;

                //#region ISession Action
                //if (_session != null)
                //{
                //    _session.Send = (msg) =>
                //    {
                //        this.Send(msg);
                //    };
                //    _session.Receive = () =>
                //    {
                //        return this.Receive();
                //    };
                //}

                //#endregion
            }
        }
        public SocketExceptionHandler OnSocketExceptionExcetion { get; set; }
        public string Identity { get; set; }
        public Socket Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

        private UserToken() :
            this(null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socket"></param>
        internal UserToken(Socket socket, ITcpServerLog iLog = null, UserTokenPool tokenPool = null)
        {
            _iLog = iLog;
            _tokenPool = tokenPool;
            _socket = socket;
            RemoteEndPoint = _socket.RemoteEndPoint as IPEndPoint;
            //socket.ReceiveTimeout = TcpServerConfig.RECEIVE_TIMEOUT;
            var networkStream = new NetworkStream(_socket);
#if ANDROID
            _reader = new TcpBinaryReader(networkStream, System.Text.Encoding.UTF8);
#else
            _reader = new BinaryReader(networkStream, System.Text.Encoding.UTF8);
#endif
            _writer = new BinaryWriter(networkStream, System.Text.Encoding.UTF8);
            //_writer.Flush();
            //_writer.AutoFlush = true;
        }

        ///// <summary>
        ///// 监听消息
        ///// </summary>
        //internal void Listener()
        //{
        //    Action clientListener = () =>
        //    {
        //        while (true)
        //        {
        //            string msg = null;

        //            try
        //            {
        //                msg = Receive();
        //            }
        //            catch
        //            {
        //            }
        //            //异常
        //            if (msg == null)
        //            {
        //                return;
        //            }                    
        //            Session.ProcessReceive(msg, this);
        //        }
        //    };

        //    _listenerThread = new Thread(() => { clientListener(); });
        //    //thread = UserThreadPool.AddThread(Identity, thread);
        //    _listenerThread.IsBackground = true;
        //    _listenerThread.Start();
        //}

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("发送的消息参数(message)不能为空");
            }
            try
            {
                if (_socket == null)
                {
                    throw new SocketException((int)SocketError.OperationAborted);
                }

                lock (_writer)
                {
                    _writer.Write(message);
                    _writer.Flush();

                    if (_iLog != null)
                    {
                        string _msg = string.Format("Token Identity:{0}", Identity);
                        _msg += string.Format("\r\nSend:{0}", message);
                        _iLog.LogDebug(_msg);
                    }

                }
            }
            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("Token Identity:{0}", Identity);
                    _msg += string.Format("\r\nSend:{0}", message);
                    _iLog.LogError(_msg, ex);
                }
                if (OnSocketExceptionExcetion != null)
                {
                    try
                    {
                        OnSocketExceptionExcetion(Identity, message);
                    }
                    catch (Exception ex_SendExcetion)
                    {
                        if (_iLog != null)
                        {
                            string _msg = string.Format("OnSocketExceptionExcetion Error:{0}\r\n{1}", ex_SendExcetion.Message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            _iLog.LogError(_msg, ex_SendExcetion);
                        }
                    }
                }

                throw ex;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <returns></returns>
        public string Receive()
        {
            try
            {
                if (_socket == null)
                {
                    throw new SocketException((int)SocketError.OperationAborted);
                }

                lock (_reader)
                {
                    string result = _reader.ReadString();
                    //while (!result.EndsWith(TcpServerConfig.Message_End) && sb.Length < 1024 * 30)
                    //{
                    //    char[] buffer = new char[1024];
                    //    _reader.Read(buffer, 0, buffer.Length);
                    //    result = string.Concat(buffer);
                    //    result = result.TrimEnd('\0');
                    //    sb.Append(result);
                    //}
                    //result = sb.ToString();

                    if (_iLog != null)
                    {
                        string _msg = string.Format("Token Identity:{0}", Identity);
                        _msg += string.Format("\r\nRecevie:{0}", result);
                        _iLog.LogDebug(_msg);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("Receive Error:{0}", ex.Message);
                    _msg += string.Format("\r\n{0} {1}", this.RemoteEndPoint, Identity);
                    _iLog.LogError(_msg, ex);
                }
                if (OnSocketExceptionExcetion != null)
                {
                    try
                    {
                        OnSocketExceptionExcetion(Identity, null);
                    }
                    catch (Exception ex_OnSocketExceptionExcetion)
                    {
                        if (_iLog != null)
                        {
                            string _msg = string.Format("OnSocketExceptionExcetion Error:{0}", ex_OnSocketExceptionExcetion.Message);
                            _msg += string.Format("\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            _iLog.LogError(_msg, ex);
                        }
                    }
                }

                throw ex;
            }
        }


        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public bool SendToUserToken(string message, string identity)
        {
            var token = _tokenPool.GetUserToken(identity);
            if (token != null)
            {
                try
                {
                    token.Send(message);
                    return true;
                }
                //异常
                catch
                {
                    _tokenPool.RemoveUserToken(identity);
                }
            }

            return false;
        }

        #region IDisposable

        /// <summary>
        /// 关闭释放
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        //是否释放资源
        private bool disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                    }
                    _reader = null;

                    if (_writer != null)
                    {
                        _writer.Close();
                    }
                    _writer = null;

                    if (_socket != null)
                    {
                        try
                        {
                            _socket.Shutdown(SocketShutdown.Both);
                            _socket.Disconnect(false);
                            _socket.Close(5000);
                        }
                        catch (Exception) { }

                        _socket = null;
                    }
                }

                //if (_listenerThread != null)
                //{
                //    try
                //    {
                //        _listenerThread.Abort();
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //}
                //_listenerThread = null;
                _lockObj = null;
                OnSocketExceptionExcetion = null;
                Identity = null;
                RemoteEndPoint = null;
            }
            this.disposed = true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalize
        /// </summary>
        ~UserToken()
        {
            Dispose(false);
        }

        #endregion
    }
}
