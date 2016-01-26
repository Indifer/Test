using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zyan.Communication;

namespace ZyanServerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ZyanComponentHost("ZyanDemo", 12800);
            host.RegisterComponent<IHellow, HellowServer>();

            Console.ReadLine();
        }
    }
}
