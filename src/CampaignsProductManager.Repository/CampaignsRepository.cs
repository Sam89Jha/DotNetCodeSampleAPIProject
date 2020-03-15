using CampaignsProductManager.Core.Exceptions;
using CampaignsProductManager.Core.Filters;
using CampaignsProductManager.Core.Interfaces;
using CampaignsProductManager.Core.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignsProductManager.Repository
{
    public sealed class CampaignsRepository : ICampaignsFRepository
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public CampaignsRepository(IConnectionManager connectionManager, ILogger logger, IConfiguration configuration)
        {
            _connectionManager = connectionManager;
            _logger = logger.ForContext<ProductsRepository>();
            _configuration = configuration;
        }

        public async Task<IEnumerable<CampaignDto>> GetCampaignAsync(CampaignFilter filter)
        {
            using (_logger.BeginTimedOperation("CampaignsRepository.GetCampaignAsync enters.."))
            {
                IEnumerable<CampaignDto> campaigns;
                DynamicParameters parameters = new DynamicParameters();
                if (!string.IsNullOrWhiteSpace(filter.CampaignId))
                {
                    parameters.Add("@campaignId", filter.CampaignId);
                }
                parameters.Add("@Skip", filter.Skip);
                parameters.Add("@Top", filter.Top);
                parameters.Add("@SortDesc", filter.SortDesc);
                if (!string.IsNullOrWhiteSpace(filter.SortBy))
                {
                    parameters.Add("@SortColumn", filter.SortBy);
                }
                if (filter.Active != null)
                {
                    parameters.Add("@Active", filter.Active);
                }
                try
                {
                    using (var cn = _connectionManager.Connection)
                    {
                        cn.Open();
                        using (var tran = cn.BeginTransaction())
                        {
                            CommandDefinition cmd = new CommandDefinition("Usp_Campaigns_Sel", parameters, tran, null, CommandType.StoredProcedure);
                            var reader = await _connectionManager.Connection.QueryMultipleAsync(cmd);
                            campaigns = reader.Read<CampaignDto>();
                            var products = reader.Read<ProductDto>();
                            campaigns.ToList().ForEach(x => x.Product = products.SingleOrDefault(y => y.CampaignId == x.Id));
                            tran.Commit();
                        }
                    }
                    return campaigns;
                }
                catch
                {
                    throw new CampaignsProductManagerException(System.Net.HttpStatusCode.InternalServerError, new Error(_configuration, ErrorItem.InternalServerError));
                }
            }
        }

        public async Task<int> SaveCampaignAsync(CampaignDto campaign)
        {
            using (_logger.BeginTimedOperation("CampaignsRepository.SaveCampaignAsync enters.."))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", campaign.Id);
                parameters.Add("@ProductId", campaign.Product.Id);
                parameters.Add("@Name", campaign.Name);
                parameters.Add("@Start", campaign.Start);
                parameters.Add("@End", campaign.End);
                parameters.Add("@Active", true);
                int result = 0;
                try
                {
                    using (var cn = _connectionManager.Connection)
                    {
                        cn.Open();
                        using (var tran = cn.BeginTransaction())
                        {
                            CommandDefinition cmd = new CommandDefinition("Usp_Campaigns_Ins", parameters, tran, null, CommandType.StoredProcedure);
                            result = await _connectionManager.Connection.ExecuteAsync(cmd);
                            tran.Commit();
                        }
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
