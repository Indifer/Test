using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void StartClient(string userName)
        {
            TTransport transport = new TSocket("localhost", 12000);
            TProtocol protocol = new TBinaryProtocol(transport);

            transport.Open();


        }
    }

   
}
