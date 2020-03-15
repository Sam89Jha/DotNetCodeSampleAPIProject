using Microsoft.Extensions.Primitives;
using Serilog;
using System.Collections.Generic;

namespace CampaignsProductManager.Core.Filters
{
    public class CampaignFilter : BaseFilter<CampaignFilter>
    {
        public CampaignFilter(ILogger logger) : base(logger) { }

        public string CampaignId { get; set; }

        public override void PopulateQueryParameters(Dictionary<string, StringValues> queryParameters)
        {
            //todo: write logic to read query params
        }
    }
}
