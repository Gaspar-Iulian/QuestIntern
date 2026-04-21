using Ecommerce.Api.Models;
using Microsoft.Data.SqlClient;

namespace Ecommerce.Api.Repositories;

public sealed class ProductRepository(SqlConnectionFactory connectionFactory) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, Name, Description, Price, ImageUrl, StockQuantity
            FROM dbo.Products
            ORDER BY Name;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var products = new List<Product>();

        while (await reader.ReadAsync(cancellationToken))
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                ImageUrl = reader.GetString(4),
                StockQuantity = reader.GetInt32(5)
            });
        }

        return products;
    }

    public async Task<IReadOnlyList<Product>> GetByIdsAsync(IReadOnlyCollection<int> productIds, CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return [];
        }

        var parameterNames = productIds.Select((_, index) => $"@ProductId{index}").ToArray();
        var sql = $"""
            SELECT Id, Name, Description, Price, ImageUrl, StockQuantity
            FROM dbo.Products
            WHERE Id IN ({string.Join(", ", parameterNames)});
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var index = 0;
        foreach (var productId in productIds)
        {
            command.Parameters.Add(new SqlParameter(parameterNames[index], productId));
            index++;
        }

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var products = new List<Product>();

        while (await reader.ReadAsync(cancellationToken))
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                ImageUrl = reader.GetString(4),
                StockQuantity = reader.GetInt32(5)
            });
        }

        return products;
    }
}
