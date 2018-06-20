using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handle
{
    public class Say : ISay<Mall>
    {
        public void Proxy(Mall mall, string message)
        {

            Console.WriteLine(mall.Name);
        }
    }
}
