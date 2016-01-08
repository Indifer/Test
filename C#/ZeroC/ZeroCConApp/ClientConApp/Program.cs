using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int status = 0;
            try
            {
                using (Ice.Communicator ic = Ice.Util.initialize(ref args))
                {
                    Ice.ObjectPrx obj = ic.stringToProxy("SimplePrinter:default -p 10000");
                    Demo.HelloPrx hello = Demo.HelloPrxHelper.checkedCast(obj);
                    if (hello == null)
                        throw new ApplicationException("Invalid proxy");

                    hello.sayHello(21535);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                status = 1;
            }
            Environment.Exit(status);
            
        }
    }
}
