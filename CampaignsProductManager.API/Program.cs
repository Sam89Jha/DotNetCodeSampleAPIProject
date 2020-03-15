using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CampaignsProductManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                Console.WriteLine($"Service running in {hostingContext.HostingEnvironment.EnvironmentName}");
                if (hostingContext.HostingEnvironment.IsDevelopment())
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                }
                config.AddJsonFile(path: "errorCodes.json", optional: false, reloadOnChange: true);
            }).UseStartup<Startup>();

    }
}
