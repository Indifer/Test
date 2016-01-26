using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZyanServerConsoleApp
{
    public class HellowServer : IHellow
    {
        public Message Create(Message msg)
        {
            msg.ID += 1;
            return msg;
        }

        public async Task<bool> Say(string msg)
        {
            Console.WriteLine(msg);
            return true;                        
        }
    }
}
