using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            UserServer userServer = new UserServer();
            UserServer.Processor processor = new UserServer.Processor();
            TServerSocket serverSocket = new TServerSocket(12000, 0, false);
            TServer server = new TSimpleServer(processor, serverSocket);

            Console.WriteLine("Starting the server...");
            server.Serve();

            Console.ReadLine();

        }
    }

}
