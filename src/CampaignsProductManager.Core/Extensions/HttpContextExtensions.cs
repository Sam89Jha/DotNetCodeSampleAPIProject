using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampaignsProductManager.Core.Exceptions;
using CampaignsProductManager.Core.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CampaignsProductManager.Core.Extensions
{
    /// <summary>
    ///     HttpContext Extensions
    /// </summary>
    public static class HttpContextExtensions
    {
        private const string Api = @"/api/";
        private const string Swagger = @"/swagger/";

        /// <summary>
        ///     Check whether the current HTTP request starts with "/api/".
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///     'true' if it starts with "/api/" otherwise 'false'.
        /// </returns>
        public static bool IsApiRequest(this HttpContext context)
        {
            var path = context.Request.Path.ToString();
            var isApi = path.StartsWith(Api, StringComparison.CurrentCultureIgnoreCase);
            return isApi;
        }

        /// <summary>
        ///     Check whether the current HTTP request is for "/swagger/"
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///     'true' if it contains "/swagger/" otherwise 'false'.
        /// </returns>
        public static bool IsSwaggerRequest(this HttpContext context)
        {
            var path = context.Request.Path.ToString();
            return path.ToLower().Contains(Swagger);
        }

        /// <summary>
        ///     Set status code and error in response
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="campaignsProductManagerException">The identity server exception.</param>
        /// <returns></returns>
        public static async Task SetErrorResponse(this HttpContext context,
            CampaignsProductManagerException campaignsProductManagerException)
        {
            var msg = string.IsNullOrWhiteSpace(campaignsProductManagerException.Error.ErrorDescription)
                ? ErrorModel.ErrorMessageDefault
                : campaignsProductManagerException.Error.ErrorDescription;
            var error = new ErrorModel((int) campaignsProductManagerException.ResponseCode)
            {
                Message = msg,
                ErrorCode = campaignsProductManagerException.Error.ErrorCode
            };
            var response = new ApiResponseModel<string>(error) {CorrelationId = context.TraceIdentifier};
            var objectResult = new ObjectResult(response) {StatusCode = (int) campaignsProductManagerException.ResponseCode};
            context.Response.ContentType = Constants.Constants.HttpResponseHeaders.ContentTypeJson;
            context.Response.StatusCode = (int) campaignsProductManagerException.ResponseCode;
            context.Response.Headers.Add(Constants.Constants.HttpResponseHeaders.HeadersAccessControlAllowOrigin,
                Constants.Constants.HttpResponseHeaders.HeadersAccessControlAllowOriginValue);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(objectResult));
        }

        /// <summary>
        ///     Gets the request headers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static string GetRequestHeaders(this HttpContext context)
        {
            var sb = new StringBuilder();

            foreach (var (key, stringValues) in context.Request.Headers)
            {
                sb.Append(key);
                sb.Append("=");
                var value = stringValues.FirstOrDefault();
                if (key.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase) &&
                    !string.IsNullOrEmpty(value))
                {
                    var tokenToLog = value;
                    if (value.LastIndexOf(".", StringComparison.Ordinal) > 0)
                        tokenToLog = value.Substring(0, value.LastIndexOf(".", StringComparison.Ordinal));
                    sb.Append(tokenToLog);
                }
                else
                {
                    sb.Append(value);
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}