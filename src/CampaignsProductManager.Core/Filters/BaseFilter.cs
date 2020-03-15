using CampaignsProductManager.Core.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Serilog;
using System.Collections.Generic;

namespace CampaignsProductManager.Core.Filters
{
    public abstract class BaseFilter<T> : IFilter
    {
        protected readonly ILogger Logger;

        protected BaseFilter(ILogger logger)
        {
            Logger = logger.ForContext<T>();
        }

        public string SortBy { get; set; }
        public bool SortDesc { get; set; }
        public int Skip { get; set; } = 0;
        public int Top { get; set; } = 1024;

        public bool? Active { get; set; } = null;

        public Dictionary<string, StringValues> GetQueryParameters(string requestUri)
        {
            Logger.Verbose(requestUri);
            return QueryHelpers.ParseQuery(requestUri);
        }

        /// <summary>
        /// Set query parameters to the filter properties
        /// </summary>
        /// <param name="queryParameters"></param>
        public abstract void PopulateQueryParameters(Dictionary<string, StringValues> queryParameters);
    }
}
