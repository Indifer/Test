using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handle
{
    public interface ISay<T>
    {
        void Proxy(T t, string message);
    }
}
