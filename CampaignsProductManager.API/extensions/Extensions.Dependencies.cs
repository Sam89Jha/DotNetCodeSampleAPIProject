using CampaignsProductManager.Core.Interfaces;
using CampaignsProductManager.Logic;
using CampaignsProductManager.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignsProductManager.API.extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Register Bus Core services
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionManager, ConnectionManager>(_ => new ConnectionManager(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IProductsRepository, ProductsRepository>();
            services.AddSingleton<ICampaignsFRepository, CampaignsRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IProductsLogic, ProductsLogic>();
            services.AddScoped<ICampaignsLogic, CampaignsLogic>();
            return services;
        }
    }
}