using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncTcpClient
{
    /// <summary>
    /// 会话
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public interface SessionHandle
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Identity { set; get; }
        /// <summary>
        /// 处理接收信息
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="token"></param>
        void ProcessReceive(string argument, TcpClientToken token);
        /// <summary>
        /// 处理发送信息
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="token"></param>
        void ProcessSend(string argument, TcpClientToken token);

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool Accredit(TcpClientToken token);

    }
}
