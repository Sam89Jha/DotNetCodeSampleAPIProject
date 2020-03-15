using CampaignsProductManager.Core.Extensions;
using CampaignsProductManager.Core.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;

namespace CampaignsProductManager.Controllers
{
    public class BaseApiController<T> : ControllerBase
    {
        /// <summary>
        ///     The logger
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseApiController{T}" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public BaseApiController(ILogger logger)
        {
            Logger = logger.ForContext<T>();
        }

        /// <summary>
        ///     Gets the correlation identifier as text.
        /// </summary>
        /// <value>
        ///     The correlation identifier as text.
        /// </value>
        protected string CorrelationIdAsText => HttpContext.TraceIdentifier;

        /// <summary>
        /// Returns the API response success with result.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="status">The status.</param>
        /// <param name="isLoggingRequired">if set to <c>true</c> [is logging required].</param>
        /// <returns></returns>
        protected ObjectResult ReturnApiResponseSuccessWithResult<T1>(T1 result, int status = StatusCodes.Status200OK, bool isLoggingRequired = true) where T1 : class
        {
            var response = new ApiResponseModel<T1>(result) { CorrelationId = CorrelationIdAsText };
            var objectResult = status == StatusCodes.Status200OK ? Ok(response) : new ObjectResult(response) { StatusCode = status };

            Logger.LogVerbose($"Response : {JsonConvert.SerializeObject(objectResult)}");


            if (null == result || typeof(T1) != typeof(Uri))
            {
                return objectResult;
            }

            var uri = (Uri)Convert.ChangeType(result, typeof(Uri));
            HttpContext.Response.Headers["Location"] = uri.AbsoluteUri;
            return objectResult;
        }

        /// <summary>
        ///     Returns the API response with error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        protected ObjectResult ReturnApiResponseWithError(ErrorModel error)
        {
            var response = new ApiResponseModel<string>(error) { CorrelationId = CorrelationIdAsText };
            return new ObjectResult(response) { StatusCode = error.HttpStatus };
        }

        /// <summary>
        ///     Returns the API response with error.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        protected ObjectResult ReturnApiResponseWithError(IEnumerable<ErrorModel> errors)
        {
            var response = new ApiResponseModel<string>(errors) { CorrelationId = CorrelationIdAsText };
            return new ObjectResult(response) { StatusCode = response.HttpStatusForResponse };
        }

        /// <summary>
        ///     Gets the URI for location response.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        protected Uri GetUriForLocationResponse(string requestUri)
        {
            var baseUri = new Uri(requestUri);
            if (!string.IsNullOrEmpty(baseUri.Query))
            {
                requestUri = requestUri.Replace(baseUri.Query, "");
            }
            if (!requestUri.EndsWith("/"))
            {
                requestUri = $"{requestUri}/";
            }
            return new Uri(requestUri);
        }
    }
}