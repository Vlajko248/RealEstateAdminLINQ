using System.Configuration;
using System.Data.SqlClient;

namespace RealEstateAdmin.Data
{
    public static class DbHelper
    {
        public static SqlConnection GetConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["tsql"];

            if (settings == null)
            {
                throw new ConfigurationErrorsException("Connection string 'tsql' was not found.");
            }

            return new SqlConnection(settings.ConnectionString);
        }
    }
}
