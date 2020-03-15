using System.Data;
using System.Data.SqlClient;
using CampaignsProductManager.Core.Interfaces;

namespace CampaignsProductManager.Repository
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly string _connectionString;

        public ConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);
    }
}
