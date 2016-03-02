using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SyncTcpClient
{
    public class TcpClientToken : IDisposable
    {
        private ITcpClientLog _iLog = null;
        private int _tcpHeartRate = 5 * 60 * 1000;
        private object _lockObj = new object();
        private TcpClient _tcpClient;
#if ANDROID
        private TcpBinaryReader _reader;
#else
        private BinaryReader _reader;
#endif
        private object _readerLock = new object();//_reader锁
        private BinaryWriter _writer;
        private object _writeLock = new object();//_write锁
        private SessionHandle _sessionHandle;

        //private Thread _listenerThread = null;

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
        }

        /// <summary>
        /// 连接Server
        /// </summary>
        /// <param name="serverHostname"></param>
        /// <param name="serverIPPoint"></param>
        public void Connect(string serverHostname, int serverIPPoint)
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)_tcpHeartRate).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)_tcpHeartRate).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);

            _tcpClient.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            _tcpClient.Connect(serverHostname, serverIPPoint);

            var netstream = _tcpClient.GetStream();
#if ANDROID
            _reader = new TcpBinaryReader(netstream, System.Text.Encoding.UTF8);
#else
            _reader = new BinaryReader(netstream, System.Text.Encoding.UTF8);
#endif
            _writer = new BinaryWriter(netstream);
        }

        //private void write7BitEncodedInt(int byteCount)
        //{
        //    while (byteCount >= 0x80)
        //    {
        //        _tcpClient.Client.Send(new byte[] { (byte)(byteCount | 0x80) });
        //        byteCount = byteCount >> 7;
        //    }

        //    _tcpClient.Client.Send(new byte[] { (byte)byteCount });
        //}

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message, int sendTimeout = 0)
        {
            try
            {
                if (_writer == null)
                {
                    return;
                }
                lock (_writeLock)
                {
                    if (_writer == null)
                    {
                        return;
                    }
                    if (sendTimeout > 0)
                    {
                        _tcpClient.SendTimeout = sendTimeout;
                    }

                    _writer.Write(message);
                    _writer.Flush();
                    _tcpClient.SendTimeout = 0;

                    if (_iLog != null)
                    {
                        string _msg = string.Format("Send:{0}", message);
                        _iLog.LogDebug(_msg);
                    }

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
        /// 接收消息
        /// </summary>
        /// <returns></returns>
        public string Receive(int receiveTimeout = 0)
        {
            try
            {
                if (_reader == null)
                {
                    return null;
                }
                lock (_readerLock)
                {
                    if (_reader == null)
                    {
                        return null;
                    }
                    if (receiveTimeout > 0)
                    {
                        _tcpClient.ReceiveTimeout = receiveTimeout;
                    }
                    string result = _reader.ReadString();
                    _tcpClient.ReceiveTimeout = 0;

                    if (_iLog != null)
                    {
                        string _msg = string.Format("Recevie:{0}", result);
                        _iLog.LogDebug(_msg);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                if (_iLog != null)
                {
                    string _msg = string.Format("Receive Error:{0}\r\n", ex.Message);
                    _msg += string.Format("{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _iLog.LogError(_msg, ex);
                }

                throw ex;
            }

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
        //            catch(Exception ex)
        //            {
        //                throw ex;
        //            }
        //            ////异常
        //            //if (msg == null)
        //            //{
        //            //    return;
        //            //}
        //            Session.ProcessReceive(msg, this);
        //        }
        //    };

        //    _listenerThread = new Thread(() => { clientListener(); });
        //    //thread = UserThreadPool.AddThread(Identity, thread);
        //    _listenerThread.IsBackground = true;
        //    _listenerThread.Start();
        //}

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

                    if (_tcpClient != null)
                    {
                        try
                        {
                            _tcpClient.Client.Shutdown(SocketShutdown.Both);
                            _tcpClient.Close();
                        }
                        catch (Exception) { }

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
