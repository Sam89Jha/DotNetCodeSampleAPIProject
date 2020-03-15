using System.Data;

namespace CampaignsProductManager.Core.Interfaces
{
    public interface IConnectionManager
    {
        IDbConnection Connection { get; }
    }
}
