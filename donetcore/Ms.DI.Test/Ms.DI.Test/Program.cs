using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ms.DI.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.BuildServiceProvider().GetRequiredService


        }
    }
}
