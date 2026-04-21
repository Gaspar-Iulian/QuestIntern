using Microsoft.Data.SqlClient;

namespace Ecommerce.Api.Repositories;

public sealed class SqlConnectionFactory(IConfiguration configuration)
{
    public SqlConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
        }

        return new SqlConnection(connectionString);
    }
}
