using CampaignsProductManager.Core.Filters;
using CampaignsProductManager.Core.Interfaces;
using CampaignsProductManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignsProductManager.Logic
{
    public sealed class ProductsLogic : IProductsLogic
    {
        private readonly IProductsRepository _productsRepository;
        public ProductsLogic(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
        public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductFilter filter)
        {
            return await _productsRepository.GetProductsAsync(filter);
        }

        public async Task<int> SaveProductAsync(ProductDto product)
        {
            return await _productsRepository.SaveProductAsync(product);
        }
    }
}
