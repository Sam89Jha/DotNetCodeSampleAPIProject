using CampaignsProductManager.Core.Exceptions;
using CampaignsProductManager.Core.Filters;
using CampaignsProductManager.Core.Interfaces;
using CampaignsProductManager.Core.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CampaignsProductManager.Repository
{
    public sealed class ProductsRepository : IProductsRepository
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ProductsRepository(IConnectionManager connectionManager, ILogger logger, IConfiguration configuration)
        {
            _connectionManager = connectionManager;
            _logger = logger.ForContext<ProductsRepository>();
            _configuration = configuration;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductFilter filter)
        {
            using (_logger.BeginTimedOperation("ProductsRepository.GetProductsAsync enters.."))
            {
                IEnumerable<ProductDto> products;
                DynamicParameters parameters = new DynamicParameters();
                if (!string.IsNullOrWhiteSpace(filter.ProductId))
                {
                    parameters.Add("@ProductId", filter.ProductId);
                }
                /* implemented all for campaigns */
                //parameters.Add("@Skip", filter.Skip);
                //parameters.Add("@Top", filter.Top);
                //parameters.Add("@SortDesc", filter.SortDesc);
                //if (!string.IsNullOrWhiteSpace(filter.SortBy))
                //{
                //    parameters.Add("@SortColumn", filter.SortBy);
                //}
                //if (filter.Active != null)
                //{
                //    parameters.Add("@Active", filter.Active);
                //}
                try
                {
                    using (var cn = _connectionManager.Connection)
                    {
                        cn.Open();
                        //using (var tran = cn.BeginTransaction())
                        //{
                        CommandDefinition cmd = new CommandDefinition("Usp_Campaigns_Sel", parameters, null, null, CommandType.StoredProcedure);
                        products = await _connectionManager.Connection.QueryAsync<ProductDto>(cmd);
                        //    tran.Commit();
                        //}
                    }
                    return products;
                }
                catch (Exception)
                {
                    throw new CampaignsProductManagerException(System.Net.HttpStatusCode.InternalServerError, new Error(_configuration, ErrorItem.InternalServerError));
                }
            }
        }

        public async Task<int> SaveProductAsync(ProductDto product)
        {
            using (_logger.BeginTimedOperation("ProductsRepository.SaveProductAsync enters.."))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", product.Id);
                parameters.Add("@Name", product.Name);
                int result = 0;
                try
                {
                    using (var cn = _connectionManager.Connection)
                    {
                        cn.Open();
                        //using (var tran = cn.BeginTransaction())
                        //{
                            CommandDefinition cmd = new CommandDefinition("Usp_Products_Ins", parameters, null, null, CommandType.StoredProcedure);
                            result = await _connectionManager.Connection.ExecuteAsync(cmd);
                        //    tran.Commit();
                        //}
                    }
                    return result;
                }
                catch
                {
                    throw new CampaignsProductManagerException(System.Net.HttpStatusCode.InternalServerError, new Error(_configuration, ErrorItem.InternalServerError));
                }
            }
        }
    }
}
