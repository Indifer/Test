using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo;

namespace ZeroCConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int status = 0;
            Ice.Communicator ic = null;
            try
            {
                ic = Ice.Util.initialize(ref args);
                Ice.ObjectAdapter adapter =
                    ic.createObjectAdapterWithEndpoints("SimplePrinterAdapter", "default -p 10000");
                Ice.Object obj = new HelloI();
                adapter.add(obj, ic.stringToIdentity("SimplePrinter"));
                adapter.activate();
                ic.waitForShutdown();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                status = 1;
            }
            if (ic != null)
            {
                // Clean up
                //
                try
                {
                    ic.destroy();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    status = 1;
                }
            }
            Environment.Exit(status);
        }
    }

    public class HelloI : Demo.HelloDisp_
    {
        public override Structure get(string name, Ice.Current current__)
        {
            throw new NotImplementedException();
        }

        public override void sayHello(int delay, Ice.Current current__)
        {
            throw new NotImplementedException();
        }

        public override void shutdown(Ice.Current current__)
        {
            throw new NotImplementedException();
        }
    }
}
