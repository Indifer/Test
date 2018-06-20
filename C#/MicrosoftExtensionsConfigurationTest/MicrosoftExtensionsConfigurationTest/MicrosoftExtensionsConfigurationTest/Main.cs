using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MicrosoftExtensionsConfigurationTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsetting.json")
                .AddCommandLine(args)

                .Build()
                ;

            void Show(IConfiguration c)
            {
                foreach (var section in c.GetChildren())
                {
                    if (section.GetChildren().Any())
                    {
                        Show(section);
                    }
                    else
                    {
                        Console.WriteLine($"{section.Key}={section.Value}");
                    }
                }

            }

            Show(configuration);

            var config = configuration.Get<Config>();
            Console.WriteLine(JsonConvert.SerializeObject(config));

        }
    }
}
