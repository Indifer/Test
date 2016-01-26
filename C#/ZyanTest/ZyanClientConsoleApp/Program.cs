using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zyan.Communication;

namespace ZyanClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ZyanConnection("tcp://localhost:12800/ZyanDemo");

            // Create HelloWorldService proxy
            var proxy = connection.CreateProxy<IHellow>();

            proxy.Say("HelloWorld").Wait();

            var msg = proxy.Create(new Message() { ID = 4 });
            Console.WriteLine(msg.ID);
            Console.ReadLine();
        }
    }
}
