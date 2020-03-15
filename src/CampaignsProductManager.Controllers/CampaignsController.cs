using CampaignsProductManager.Core.Filters;
using CampaignsProductManager.Core.Interfaces;
using CampaignsProductManager.Core.Models;
using CampaignsProductManager.Core.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CampaignsProductManager.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CampaignsController : BaseApiController<CampaignsController>
    {
        private readonly ICampaignsLogic _campaignsLogic;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CampaignsController(ILogger logger,
            ICampaignsLogic campaignsLogic, IHttpContextAccessor httpContextAccessor) : base(logger)
        {
            _campaignsLogic = campaignsLogic;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get List of campaigns
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetCampaignAsync()
        {
            var filter = new CampaignFilter(Logger);
            var queryParameters = filter.GetQueryParameters(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            filter.PopulateQueryParameters(queryParameters);
            return ReturnApiResponseSuccessWithResult(await _campaignsLogic.GetCampaignAsync(filter));
        }

        /// <summary>
        /// Save Campaign
        /// </summary>
        /// <param name="campaign">campaign</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> SaveCampaignAsync(CampaignDto campaign)
        {
            int res = await _campaignsLogic.SaveCampaignAsync(campaign);
            if (res > 0)
            {
                return ReturnApiResponseSuccessWithResult("created", (int)HttpStatusCode.Created);
            }
            return ReturnApiResponseWithError(new Core.ResponseModel.ErrorModel((int)HttpStatusCode.InternalServerError));
        }
    }
}
