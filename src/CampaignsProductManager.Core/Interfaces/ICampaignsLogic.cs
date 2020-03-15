using CampaignsProductManager.Core.Filters;
using CampaignsProductManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignsProductManager.Core.Interfaces
{
    public interface ICampaignsLogic
    {
        /// <summary>
        /// GetCampaignsAsync
        /// </summary>
        /// <param name="filter">CampaignFilter</param>
        /// <returns>IEnumerable<CampaignDto></returns>
        Task<IEnumerable<CampaignDto>> GetCampaignAsync(CampaignFilter filter);

        /// <summary>
        /// SaveCampaignAsync
        /// </summary>
        /// <param name="Campaign">CampaignDto</param>
        /// <returns>int</returns>
        Task<int> SaveCampaignAsync(CampaignDto Campaign);
    }
}
