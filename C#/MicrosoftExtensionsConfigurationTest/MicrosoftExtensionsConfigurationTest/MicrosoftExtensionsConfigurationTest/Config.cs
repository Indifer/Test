using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftExtensionsConfigurationTest
{
    public class Config
    {
        public string ServiceHost { get; set; }

        public Consul Consul { get; set; }
    }

    public class Consul
    {
        public string Host { get; set; }
    }
}
