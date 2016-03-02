using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncTcpServer
{
    /// <summary>
    /// 授权
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    public interface ServerHandle
    {
        ///// <summary>
        ///// 发送
        ///// </summary>
        //public Action<string> Send;
        ///// <summary>
        ///// 接收
        ///// </summary>
        //public Func<string> Receive;
        ///// <summary>
        ///// 授权失败
        ///// </summary>
        ///// <returns></returns>
        //string AccreditFailSendMessage();
        ///// <summary>
        ///// 授权邀请
        ///// </summary>
        //string AccreditInviteSendMessage();

        /// <summary>
        /// 授权
        /// </summary>
        /// <returns></returns>
        SessionHandle Accredit(UserToken token);
    }
}
