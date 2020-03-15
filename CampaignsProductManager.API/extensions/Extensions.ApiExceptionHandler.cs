using CampaignsProductManager.Core.Exceptions;
using CampaignsProductManager.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Net;

namespace CampaignsProductManager.API.extensions
{
    /// <summary>
    /// Extension for configuring exceptions
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Configures exception handlers
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder appBuilder, ILogger logger, IConfiguration configuration)
        {
            return appBuilder
                .UseWhen(context => context.IsApiRequest(), app => { app.UseApiExceptionHandler(logger, configuration); })
                .UseWhen(context => !context.IsApiRequest(), app =>
                {
                    app.UseExceptionHandler(options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                context.Response.ContentType = "text/html";
                                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                                if (exceptionFeature != null)
                                {
                                    string routeWhereExceptionOccurred = exceptionFeature.Path;
                                    Exception exceptionThatOccurred = exceptionFeature.Error;
                                    logger.Error(exceptionThatOccurred, $"Path:{routeWhereExceptionOccurred}");
                                    var errorPageContent = $"<h3>Oops, something went wrong. Please try again later!</h3>" +
                                                           $"<div>Please contact the System Administrator for further assistance.</div><br/>" +
                                                           $"<div><b>ErrorId:</b> {context.Request.HttpContext.TraceIdentifier}</div>";

                                    await context.Response.WriteAsync(errorPageContent).ConfigureAwait(false);
                                }
                            });
                    });
                });
        }

        /// <summary>
        /// Creates exception handler for api requests.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder appBuilder, ILogger logger, IConfiguration configuration)
        {
            appBuilder.UseExceptionHandler(
                options =>
                {
                    options.Run(async context =>
                    {
                        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        if (exceptionFeature != null)
                        {
                            string routeWhereExceptionOccurred = exceptionFeature.Path;
                            Exception exceptionThatOccurred = exceptionFeature.Error;

                            if (!(exceptionThatOccurred is CampaignsProductManagerException campaignsProductManagerException))
                            {
                                logger.Error(exceptionThatOccurred, exceptionThatOccurred.Message);
                                var error = new Error(configuration, ErrorItem.InternalServerError);
                                campaignsProductManagerException = new CampaignsProductManagerException(HttpStatusCode.InternalServerError, error);
                            }

                            logger.Error(campaignsProductManagerException, routeWhereExceptionOccurred);
                            logger.Error("Headers : " + context.GetRequestHeaders());

                            await context.SetErrorResponse(campaignsProductManagerException);
                        }
                    });
                }
            );
            return appBuilder;
        }
    }
}
