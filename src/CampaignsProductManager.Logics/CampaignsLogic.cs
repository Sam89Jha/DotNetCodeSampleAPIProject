using CampaignsProductManager.Core.Filters;
using CampaignsProductManager.Core.Interfaces;
using CampaignsProductManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignsProductManager.Logic
{
    public sealed class CampaignsLogic : ICampaignsLogic
    {
        private readonly ICampaignsFRepository _campaignsFRepository;
        public CampaignsLogic(ICampaignsFRepository campaignsFRepository)
        {
            _campaignsFRepository = campaignsFRepository;
        }
        public async Task<IEnumerable<CampaignDto>> GetCampaignAsync(CampaignFilter filter)
        {
            return await _campaignsFRepository.GetCampaignAsync(filter);
        }

        public async Task<int> SaveCampaignAsync(CampaignDto campaign)
        {
            return await _campaignsFRepository.SaveCampaignAsync(campaign);
        }
    }
}
