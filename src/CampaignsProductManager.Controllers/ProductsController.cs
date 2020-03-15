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
    public class ProductsController : BaseApiController<ProductsController>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductsLogic _productsLogic;
        public ProductsController(ILogger logger,
            IHttpContextAccessor httpContextAccessor, IProductsLogic productsLogic) : base(logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _productsLogic = productsLogic;
        }

        [ProducesResponseType(typeof(ApiResponseModel<List<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var filter = new ProductFilter(Logger);
            var queryParameters = filter.GetQueryParameters(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            filter.PopulateQueryParameters(queryParameters);
            return ReturnApiResponseSuccessWithResult(await _productsLogic.GetProductsAsync(filter));
        }

        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> SaveProductAsync([FromBody]ProductDto product)
        {
            int res = await _productsLogic.SaveProductAsync(product);
            if (res > 0)
            {
                return ReturnApiResponseSuccessWithResult("created", (int)HttpStatusCode.Created);
            }
            return ReturnApiResponseWithError(new Core.ResponseModel.ErrorModel((int)HttpStatusCode.InternalServerError));
        }
    }
}