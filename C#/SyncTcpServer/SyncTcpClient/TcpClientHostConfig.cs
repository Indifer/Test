using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncTcpClient
{
    /// <summary>
    /// TcpClientHostConfig
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public class TcpClientHostConfig
    {
        public string ServerHostname { get; private set; }
        public int ServerIPPoint { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverHostname"></param>
        /// <param name="serverIpPoint"></param>
        /// <param name="reConnNum"></param>
        /// <param name="maxReConnNum"></param>
        public TcpClientHostConfig(string serverHostname, int serverIpPoint)
        {
            ServerHostname = serverHostname;
            ServerIPPoint = serverIpPoint;
        }
    }
}
