using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace CampaignsProductManager.API.extensions
{
    public static partial class Extensions
    {
        private const string ApplicationBasePath = "ApplicationBasePath";
        private const string AppVersion = "v1";
        private const string SwaggerJsonfile = @"swagger/{documentName}/swagger.json";
        private const string SwaggerUi = @"swagger/ui/index";
        private const string SwaggerVersionJsonfile = @"/swagger/" + AppVersion + "/swagger.json";

        /// <summary>
        /// Setup swagger.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static IServiceCollection SetupSwagger(this IServiceCollection services, ILogger logger)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            logger.Information("Adding Swagger Gen Service");

            services.AddSwaggerGen(options =>
            {
                try
                {
                    options.DocInclusionPredicate((version, apiDescription) =>
                    {
                        string[] values = apiDescription.RelativePath.Split('/');
                        apiDescription.RelativePath = string.Join("/", values).Replace("v{version}", version);

                        ApiParameterDescription versionParameter = apiDescription.ParameterDescriptions
                            .SingleOrDefault(p => p.Name == "version");

                        if (versionParameter != null)
                        {
                            apiDescription.ParameterDescriptions.Remove(versionParameter);
                        }

                        return true;
                    });

                    string buildVersion = typeof(Startup).GetTypeInfo().Assembly.GetName().Version.ToString();
                    options.SwaggerDoc(AppVersion, new Info
                    {
                        Version = AppVersion,
                        Title = "Campaigns and Products Management Service",
                        Description = $"<br><br>Build: {buildVersion}<br>Copyright © Campaigns and Products Managements.",
                        TermsOfService = "None"
                    });

                    //get the xml file path from where xml summary is to be loaded using swagger json and ui.
                    string docPath = PlatformServices.Default.Application.ApplicationBasePath;
                    logger.Information("Swagger including Xml documents [path: '{0}']", docPath);
                    var directoryInfo = new DirectoryInfo(docPath);

                    //Validate if directory exists and then iterate through all the xml files to fetch xml comments
                    if (!directoryInfo.Exists)
                    {
                        return;
                    }

                    var allXmlFiles = directoryInfo.EnumerateFiles("*.xml");

                    foreach (var xmlFile in allXmlFiles)
                    {
                        var fullXmlFileName = xmlFile.FullName;
                        options.IncludeXmlComments(fullXmlFileName);
                        logger.Information("Added to swagger the document file '{0}'.", fullXmlFileName);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while setting options for 'AddSwaggerGen'.");
                    throw;
                }
            });

            return services;
        }

        /// <summary>
        /// Configure swagger.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder appBuilder, IConfiguration configuration, ILogger logger)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            string swaggerRouteTemplate = SwaggerJsonfile;

            appBuilder.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerRouteTemplate;
            });

            logger.Information("The Swagger endpoint to generate JSON doc has been added [RouteTemplate: '{0}'].", swaggerRouteTemplate);

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            string swaggerUiRoute = SwaggerUi;
            string swaggerDoc = SwaggerVersionJsonfile;
            string applicationBasePath = configuration[ApplicationBasePath];
            if (!string.IsNullOrWhiteSpace(applicationBasePath))
            {
                swaggerDoc = $"/{applicationBasePath}{swaggerDoc}";
            }
            logger.Information("The Swagger UI will use [swaggerUiRoute: '{0}'] [swaggerDoc: '{1}'].", swaggerUiRoute, swaggerDoc);

            appBuilder.UseSwaggerUI(options =>
            {
                options.RoutePrefix = swaggerUiRoute;
                options.SwaggerEndpoint(swaggerDoc, AppVersion);
            });
            logger.Information("The Swagger UI has been added [swaggerUiRoute: '{0}'] [swaggerDoc: '{1}'].", swaggerUiRoute, swaggerDoc);
            return appBuilder;
        }
    }
}
