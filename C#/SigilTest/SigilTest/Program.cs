using Sigil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sigil;
using System.Reflection;
using System.Reflection.Emit;

namespace SigilTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var e1 = Emit<Func<Task<int>>>.NewDynamicMethod("test");


        }
    }
}
