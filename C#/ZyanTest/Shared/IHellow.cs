using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IHellow
    {
        Task<bool> Say(string msg);
        Message Create(Message msg);
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Message
    {
        public long ID { get; set; }

        public string Name { get; set; }

    }
}
