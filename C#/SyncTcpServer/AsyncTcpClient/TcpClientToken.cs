using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTcpClient
{
    public class TcpClientToken : IDisposable
    {
        private ITcpClientLog _iLog = null;
        private int _tcpHeartRate = 5 * 60 * 1000;
        private object _lockObj = new object();
        private TcpClient _tcpClient;
        //private Socket _socket;
        private SessionHandle _sessionHandle;

        /// <summary>
        /// Socket异常
        /// </summary>
        public Action<Exception> SocketErrorAction;

        private byte[] buffer;
        private List<byte> bufferList;

        private static bool IsConnectionSuccessful = false;
        private static Exception socketexception;
        private static ManualResetEvent TimeoutObject;

        public TcpClient TcpClient
        {
            get { return _tcpClient; }
            set { _tcpClient = value; }
        }

        public SessionHandle Session
        {
            get { return _sessionHandle; }
            set
            {
                _sessionHandle = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sessionHandle"></param>
        public TcpClientToken(SessionHandle sessionHandle, ITcpClientLog tcpClientLog)
        {
            _sessionHandle = sessionHandle;
            _iLog = tcpClientLog;

            _tcpClient = new TcpClient();
            //_socket = _tcpClient.Client;

            buffer = new byte[1024 * 8];
            bufferList = new List<byte>(1024 * 8);

            TimeoutObject = new ManualResetEvent(false);
        }

        /// <summary>
        /// 连接Server
        /// </summary>
        /// <param name="serverHostname"></param>
        /// <param name="serverIPPoint"></param>
        public void Connect(string serverHostname, int serverIPPoint, int timeoutMiliSecond = 10000)
        {
            TimeoutObject.Reset();
            socketexception = null;

            //uint dummy = 0;
            //byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            //BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            //BitConverter.GetBytes((uint)_tcpHeartRate).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            //BitConverter.GetBytes((uint)_tcpHeartRate).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);

            //_tcpClient.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

            //_tcpClient.BeginConnect(serverHostname, serverIPPoint, new AsyncCallback(ConnectCallBack), _tcpClient);
            _tcpClient.Connect(serverHostname, serverIPPoint);

            /*
            if (TimeoutObject.WaitOne(timeoutMiliSecond, false))
            {
                if (IsConnectionSuccessful)
                {
                }
                else
                {
                    throw socketexception;
                }
            }
            else
            {
                try
                {
                    //_tcpClient.Client.Shutdown(SocketShutdown.Both);

                    if (_tcpClient.Client != null)
                    {
                        _tcpClient.Client.Close(3000);
                    }
                    _tcpClient.Close();
                }
                catch (Exception ex)
                {

                }
                throw new TimeoutException("TimeOut Exception");
            }*/

            //_tcpClient.Connect(serverHostname, serverIPPoint);

        }

        private static void ConnectCallBack(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;
                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);
                    IsConnectionSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message, int sendTimeout = 0)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("发送的消息参数(message)不能为空");
            }

            if (_tcpClient.Client == null)
            {
                throw new SocketException((int)SocketError.OperationAborted);
            }

            if (!_tcpClient.Connected)
            {
                throw new SocketException((int)SocketError.NotConnected);
            }

            int byteCount;
            byte[] headProtocol;
            byte[] buffer;
            byte[] data;
            try
            {

                byteCount = Encoding.UTF8.GetByteCount(message);
                headProtocol = AsyncTcpServer.HeadProtocol.Get7BitEncodedInt(byteCount);
                buffer = new byte[headProtocol.Length + byteCount];
                data = Encoding.UTF8.GetBytes(message);
                var j = headProtocol.Length;
                for (int i = 0; i < j; i++)
                {
                    buffer[i] = headProtocol[i];
                }
                for (int i = 0, k = data.Length; i < k; i++)
                {
                    buffer[i + j] = data[i];
                }

                _tcpClient.Client.SendTimeout = sendTimeout;
                //_tcpClient.Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, null);
                _tcpClient.Client.Send(buffer, 0, buffer.Length, SocketFlags.None);

                if (_iLog != null)
                {
                    string _msg = string.Format("Send:{0}", message);
                    _iLog.LogDebug(_msg);
                }

            }

            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("Send Error:{0}\r\n{1}", message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _iLog.LogError(_msg, ex);
                }

                throw ex;
            }
            finally
            {
                if (_tcpClient != null)
                {
                    _tcpClient.SendTimeout = 0;
                }
            }

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public Task SendAsync(string message, int sendTimeout = 0)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("发送的消息参数(message)不能为空");
            }

            if (_tcpClient.Client == null)
            {
                throw new SocketException((int)SocketError.OperationAborted);
            }

            if (!_tcpClient.Connected)
            {
                throw new SocketException((int)SocketError.NotConnected);
            }

            int byteCount;
            byte[] headProtocol;
            byte[] buffer;
            byte[] data;
            try
            {

                byteCount = Encoding.UTF8.GetByteCount(message);
                headProtocol = AsyncTcpServer.HeadProtocol.Get7BitEncodedInt(byteCount);
                buffer = new byte[headProtocol.Length + byteCount];
                data = Encoding.UTF8.GetBytes(message);
                var j = headProtocol.Length;
                for (int i = 0; i < j; i++)
                {
                    buffer[i] = headProtocol[i];
                }
                for (int i = 0, k = data.Length; i < k; i++)
                {
                    buffer[i + j] = data[i];
                }

                _tcpClient.Client.SendTimeout = sendTimeout;
                //_tcpClient.Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, null);
                return Task.Factory.FromAsync(_tcpClient.Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null), SendCallBack).ContinueWith(x =>
                {
                    if (_iLog != null)
                    {
                        string _msg = string.Format("Send:{0}", message);
                        _iLog.LogDebug(_msg);
                    }
                });
                //_tcpClient.Client.Send(buffer, 0, buffer.Length, SocketFlags.None);


            }

            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("Send Error:{0}\r\n{1}", message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _iLog.LogError(_msg, ex);
                }

                throw ex;
            }
            finally
            {
                if (_tcpClient != null)
                {
                    _tcpClient.SendTimeout = 0;
                }
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
                //Socket sockt = result.AsyncState as Socket;

                if (_tcpClient.Client != null)
                {
                    _tcpClient.Client.EndSend(result);
                }

            }
            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("SendCallBack:{0}", ex.Message);
                    _iLog.LogError(_msg, ex);
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <returns></returns>
        public void BeginReceive()
        {
            try
            {
                if (_tcpClient.Client == null)
                {
                    throw new SocketException((int)SocketError.OperationAborted);
                }

                _tcpClient.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecevieCallBack, _tcpClient.Client);
            }
            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("BeginReceive Error:{0}", ex.Message);
                    _iLog.LogError(_msg, ex);
                }
                throw ex;
            }

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
                    //receiveCount = sk.EndReceive(result);
                }
                catch (Exception ex)
                {
                    //(ex as SocketError) == SocketError.cl
                    if (_iLog != null)
                    {
                        string _msg = string.Format("Token Identity:{0}", _sessionHandle.Identity);
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
                }

                buffer = new byte[1024 * 8];

                if (_tcpClient.Client == null)
                {
                    return;
                    //throw new SocketException((int)SocketError.OperationAborted);
                }

                try
                {
                    //Socket sockt = result.AsyncState as Socket;

                    if (_tcpClient.Client != null && bufferList.Count > 0)
                    {
                        _tcpClient.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecevieCallBack, _tcpClient.Client);
                    }
                    else
                    {
                        //System.Threading.Tasks.Task.Factory.StartNew(() =>
                        //{
                        //    if (SocketErrorAction != null)
                        //    {
                        //        SocketErrorAction(new SocketException((int)SocketError.OperationAborted));
                        //    }
                        //}).LogExceptions((e) => { _iLog.LogError(e.Message, e, "task.txt"); });
                        //SocketErrorAction.BeginInvoke(new SocketException((int)SocketError.OperationAborted), null, null);
                    }

                }
                catch (Exception ex)
                {
                    if (_iLog != null)
                    {
                        string _msg = string.Format("Token Identity:{0}", _sessionHandle.Identity);
                        _msg += string.Format("\r\nRecevieCallBack:{0}", ex.Message);
                        _iLog.LogError(_msg, ex);
                    }
                }

                BinaryInputHandler();
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
                string data;
                lock (bufferList)
                {
                    int byteCount = AsyncTcpServer.HeadProtocol.Read7BitEncodedInt(bufferList, out offset);

                    if (bufferList.Count - offset < byteCount)
                        return;

                    data = System.Text.Encoding.UTF8.GetString(bufferList.ToArray(), offset, byteCount);

                    bufferList.RemoveRange(0, offset + byteCount);
                }

                if (_sessionHandle != null)
                {
                    _sessionHandle.ProcessReceive(data, this);
                }

                if (_iLog != null)
                {
                    string _msg = string.Format("ProcessReceive:{0}", data);
                    _iLog.LogDebug(_msg);
                }

                if (bufferList.Count > 0)
                {
                    BinaryInputHandler();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("出现异常" + ex.ToString());

                if (_iLog != null)
                {
                    var _msg = string.Format("BinaryInputHandler:{0}", ex.Message);
                    _iLog.LogError(_msg, ex);
                }

                bufferList.Clear();
                return;
            }
        }

        public bool IsConnected()
        {
            if (TcpClient != null && TcpClient.Client != null)
            {
                return TcpClient.Client.Connected;
            }
            else return false;
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
                    if (_tcpClient != null)
                    {
                        try
                        {
                            _tcpClient.Client.Shutdown(SocketShutdown.Both);
                            //_tcpClient.Client.Disconnect(false);
                            if (_tcpClient.Client != null)
                            {
                                _tcpClient.Client.Close(100);
                            }
                            _tcpClient.Close();
                        }
                        catch (Exception)
                        {
                            ;
                        }

                        //_tcpClient.SendTimeout = 1000;
                        //_tcpClient.ReceiveTimeout = 1000;
                        _tcpClient = null;
                    }
                }
                _lockObj = null;
                //OnSocketExceptionExcetion = null;
                //Identity = null;
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
        ~TcpClientToken()
        {
            Dispose(false);
        }

        #endregion
    }
}
