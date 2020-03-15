using Microsoft.Extensions.Primitives;
using Serilog;
using System.Collections.Generic;

namespace CampaignsProductManager.Core.Filters
{
    public class ProductFilter : BaseFilter<ProductFilter>
    {
        public ProductFilter(ILogger logger) : base(logger) { }

        public string ProductId { get; set; }

        public override void PopulateQueryParameters(Dictionary<string, StringValues> queryParameters)
        {
            //todo: write logic to read query params
        }
    }
}
