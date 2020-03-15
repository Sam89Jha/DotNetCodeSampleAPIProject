using CampaignsProductManager.Core.Filters;
using CampaignsProductManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignsProductManager.Core.Interfaces
{
    public interface IProductsLogic
    {
        /// <summary>
        /// GetProductsAsync
        /// </summary>
        /// <param name="filter">ProductFilter</param>
        /// <returns>IEnumerable<ProductDto></returns>
        Task<IEnumerable<ProductDto>> GetProductsAsync(ProductFilter filter);

        /// <summary>
        /// SaveProductAsync
        /// </summary>
        /// <param name="product">ProductDto</param>
        /// <returns>int</returns>
        Task<int> SaveProductAsync(ProductDto product);
    }
}
