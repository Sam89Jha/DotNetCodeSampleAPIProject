using CampaignsProductManager.API.extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace CampaignsProductManager.API
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private ILogger Logger { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = GetLogger();
            Logger = Log.Logger.ForContext<Startup>();

            Logger.Information("Got Log Configuration");

            services
                .AddSingleton(Log.Logger)
                .AddCors()
                .AddDependencies(Configuration)
                .SetupSwagger(Log.Logger)
                .SetupApiVersioning()
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, ILogger logger)
        {
            loggerFactory.AddSerilog();
            app
                .UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials())
                .UseHttpsRedirection()
                .ConfigureExceptionHandler(Logger, Configuration)
                .ConfigureSwagger(Configuration, logger)
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseMvcWithDefaultRoute();
        }
    }
}