using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MFlix.HttpApi
{
    public sealed class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
