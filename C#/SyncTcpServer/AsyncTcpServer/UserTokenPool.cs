using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsyncTcpServer
{
    /// <summary>
    /// 用户tokenpool
    /// <remarks>code by LiangYi</remarks>
    /// </summary>
    internal class UserTokenPool
    {
        internal ConcurrentDictionary<string, UserToken> userTokens = new ConcurrentDictionary<string, UserToken>();
        //private ConcurrentQueue<Socket> socketQueue = new ConcurrentQueue<Socket>();

        /// <summary>
        /// 添加用户token
        /// </summary>
        /// <param name="token"></param>
        public void AddUserToken(UserToken token)
        {
            token = userTokens.AddOrUpdate(token.Identity, token, (key, val) =>
            {
                val.Close();
                val = null;
                return token;
            });
        }

        /// <summary>
        /// 获取用户token
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public UserToken GetUserToken(string identity)
        {
            if (!string.IsNullOrWhiteSpace(identity))
            {
                UserToken token = null;
                if (userTokens.TryGetValue(identity, out token))
                {
                    return token;
                }
            }
            return null;
        }

        /// <summary>
        /// 移除用户token
        /// </summary>
        /// <param name="identity"></param>
        public bool RemoveUserToken(string identity)
        {
            if (!string.IsNullOrWhiteSpace(identity))
            {
                UserToken token = null;
                if (userTokens.TryRemove(identity, out token))
                {
                    token.Close();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return userTokens.Count();
        }

        /// <summary>
        /// 清除所有
        /// </summary>
        public void Clear()
        {
            foreach (var t in userTokens)
            {
                t.Value.Close();
            }
            userTokens.Clear();
        }
    }
}
