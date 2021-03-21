using System;
using System.IO;
using MFlix.Data.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MFlix.DataTests.DependencyInjection
{
    public static class TestServicesSetup
    {
        public static IServiceProvider ConfigureTestServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return new ServiceCollection()
                .ConfigureDataServices(configuration)
                .BuildServiceProvider();
        }
    }
}
