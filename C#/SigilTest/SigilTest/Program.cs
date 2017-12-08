using Sigil;
using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace SigilTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var emit = Emit<Func<Task<int>>>.NewDynamicMethod("test");

        }
    }
}
