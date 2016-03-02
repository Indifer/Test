using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsyncTcpServer
{
    public delegate void SocketExceptionHandler(string identity, object obj);
    /// <summary>
    /// 用户token
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class UserToken : IDisposable
    {
        private ITcpServerLog _iLog = null;
        //private SocketError socketError;
        private UserTokenPool _tokenPool = null;
        private Socket _socket;
        private object _lockObj = new object();

        private SessionHandle _session { get; set; }

        /// <summary>
        /// 最近活动时间
        /// </summary>
        public DateTime ActivityTime { get; set; }
        /// <summary>
        /// Socket异常
        /// </summary>
        public Action<UserToken, Exception> SocketErrorAction;

        public Action<int> SendSuccessAction { get; set; }

        public Action<int> ReceiveSuccessAction { get; set; }

        //private Thread _listenerThread = null;
        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }

        public SessionHandle Session
        {
            get { return _session; }
            set
            {
                _session = value;
            }
        }
        public SocketExceptionHandler OnSocketExceptionExcetion { get; set; }

        /// <summary>
        /// 数据输入处理
        /// </summary>
        public Action<string> DataInputHandler { get; set; }

        private byte[] buffer;
        private List<byte> bufferList;

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
            buffer = new byte[1024];
            bufferList = new List<byte>(1024);

            BeginReceive();

            //socket.ReceiveTimeout = TcpServerConfig.RECEIVE_TIMEOUT;
            //var networkStream = new NetworkStream(_socket);

            //_writer.Flush();
            //_writer.AutoFlush = true;
        }

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

                int byteCount = Encoding.UTF8.GetByteCount(message);
                byte[] headProtocol = HeadProtocol.Get7BitEncodedInt(byteCount);
                byte[] buffer = new byte[headProtocol.Length + byteCount];
                byte[] data = Encoding.UTF8.GetBytes(message);
                int j = headProtocol.Length;
                for (int i = 0; i < j; i++)
                {
                    buffer[i] = headProtocol[i];
                }
                for (int i = 0, k = data.Length; i < k; i++)
                {
                    buffer[i + j] = data[i];
                }

                _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, _socket);

                //发送数据事件
                if (SendSuccessAction != null)
                {
                    SendSuccessAction(buffer.Length);
                }

                if (_iLog != null)
                {
                    string _msg = string.Format("Token Identity:{0}", Identity);
                    _msg += string.Format("\r\nSend:{0}", message);
                    _iLog.LogDebug(_msg);
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
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void SendCallBack(IAsyncResult result)
        {
            try
            {
                Socket sk = result.AsyncState as Socket;

                if (sk != null)
                {
                    sk.EndSend(result);
                }

            }
            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("Token Identity:{0}", Identity);
                    _msg += string.Format("\r\nSendError:{0}", ex.Message);
                    _iLog.LogDebug(_msg);
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <returns></returns>
        private void BeginReceive()
        {
            if (_socket == null)
            {
                throw new SocketException((int)SocketError.OperationAborted);
            }

            try
            {
                //Socket sockt = result.AsyncState as Socket;

                if (_socket != null)
                {
                    _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecevieCallBack, _socket);
                }

            }
            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("Token Identity:{0}", Identity);
                    _msg += string.Format("\r\nBeginReceive:{0}", ex.Message);
                    _iLog.LogDebug(_msg);
                }
            }

            //lock (_reader)
            //{
            //    string result = _reader.ReadString();
            //    //while (!result.EndsWith(TcpServerConfig.Message_End) && sb.Length < 1024 * 30)
            //    //{
            //    //    char[] buffer = new char[1024];
            //    //    _reader.Read(buffer, 0, buffer.Length);
            //    //    result = string.Concat(buffer);
            //    //    result = result.TrimEnd('\0');
            //    //    sb.Append(result);
            //    //}
            //    //result = sb.ToString();
            //
            //    if (_iLog != null)
            //    {
            //        string _msg = string.Format("Token Identity:{0}", Identity);
            //        _msg += string.Format("\r\nRecevie:{0}", result);
            //        _iLog.LogDebug(_msg);
            //    }
            //
            //    return result;
            //}

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void RecevieCallBack(IAsyncResult result)
        {
            Socket sk = (Socket)result.AsyncState;
            if (sk != null && result != null && result.IsCompleted)
            {
                int receiveCount;
                SocketError errorCode = SocketError.Success;
                try
                {
                    receiveCount = sk.EndReceive(result, out errorCode);
                    this.ActivityTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    if (_iLog != null)
                    {
                        string _msg = string.Format("Token Identity:{0}", Identity);
                        _msg += string.Format("\r\nEndReceive:{0}", ex.Message);
                        _iLog.LogError(_msg, ex);
                    }

                    bufferList.Clear();
                    return;
                }

                if (errorCode != SocketError.Success || receiveCount == 0)
                {
                    bufferList.Clear();
                    return;
                }

                lock (bufferList)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        if (buffer[i] == 0)
                        {
                            break;
                        }

                        bufferList.Add(buffer[i]);
                    }

                    //接收数据事件
                    if (ReceiveSuccessAction != null)
                    {
                        ReceiveSuccessAction(buffer.Length);
                    }

                    buffer = new byte[1024];

                    if (sk == null)
                    {
                        return;
                        //throw new SocketException((int)SocketError.OperationAborted);
                    }

                    try
                    {
                        //Socket sockt = result.AsyncState as Socket;

                        if (sk != null && bufferList.Count > 0)
                        {
                            sk.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecevieCallBack, sk);
                        }
                        else
                        {
                            //System.Threading.Tasks.Task.Factory.StartNew(() =>
                            //{
                            //    if (SocketErrorAction != null)
                            //    {
                            //        SocketErrorAction(this, new SocketException((int)SocketError.OperationAborted));
                            //    }
                            //}).LogExceptions((e) => { _iLog.LogError(e.Message, e, "task.txt"); });
                            //SocketErrorAction.BeginInvoke(new SocketException((int)SocketError.OperationAborted), null, null);
                        }

                    }
                    catch (Exception ex)
                    {
                        if (_iLog != null)
                        {
                            string _msg = string.Format("Token Identity:{0}", Identity);
                            _msg += string.Format("\r\nRecevieCallBack:{0}", ex.Message);
                            _iLog.LogDebug(_msg);
                        }
                    }

                    BinaryInputHandler();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void BinaryInputHandler()
        {
            int offset;
            try
            {
                if (bufferList.Count == 0)
                {
                    return;
                }
                int byteCount = HeadProtocol.Read7BitEncodedInt(bufferList, out offset);

                if (bufferList.Count - offset < byteCount)
                    return;

                string data = System.Text.Encoding.UTF8.GetString(bufferList.ToArray(), offset, byteCount);

                lock (bufferList)
                {
                    bufferList.RemoveRange(0, offset + byteCount);
                }

                if (_session != null)
                {
                    _session.ProcessReceive(data, this);
                }

                if (bufferList.Count > 0)
                {
                    BinaryInputHandler();
                }
            }
            catch (Exception ex)
            {
                bufferList.Clear();
                this.Close();
                return;
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
                    //if (_reader != null)
                    //{
                    //    _reader.Close();
                    //}
                    //_reader = null;

                    //if (_writer != null)
                    //{
                    //    _writer.Close();
                    //}
                    //_writer = null;

                    if (_socket != null)
                    {
                        try
                        {
                            _socket.Shutdown(SocketShutdown.Both);
                            //_socket.Disconnect(false);
                            _socket.Close(100);
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
