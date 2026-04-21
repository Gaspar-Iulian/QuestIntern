using Ecommerce.Api.Models;
using Microsoft.Data.SqlClient;

namespace Ecommerce.Api.Repositories;

public sealed class OrderRepository(SqlConnectionFactory connectionFactory) : IOrderRepository
{
    public async Task<OrderCreated> CreateAsync(OrderDraft order, CancellationToken cancellationToken)
    {
        const string orderSql = """
            INSERT INTO dbo.Orders
                (UserId, ShippingFullName, ShippingAddressLine1, ShippingCity, ShippingPostalCode, ShippingCountry, TotalAmount)
            OUTPUT inserted.Id, inserted.TotalAmount, inserted.CreatedAtUtc
            VALUES
                (@UserId, @ShippingFullName, @ShippingAddressLine1, @ShippingCity, @ShippingPostalCode, @ShippingCountry, @TotalAmount);
            """;

        const string itemSql = """
            INSERT INTO dbo.OrderItems
                (OrderId, ProductId, ProductName, Quantity, UnitPrice, LineTotal)
            VALUES
                (@OrderId, @ProductId, @ProductName, @Quantity, @UnitPrice, @LineTotal);
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await using var orderCommand = connection.CreateCommand();
            orderCommand.Transaction = (SqlTransaction)transaction;
            orderCommand.CommandText = orderSql;
            orderCommand.Parameters.Add(new SqlParameter("@UserId", (object?)order.UserId ?? DBNull.Value));
            orderCommand.Parameters.Add(new SqlParameter("@ShippingFullName", order.ShippingFullName));
            orderCommand.Parameters.Add(new SqlParameter("@ShippingAddressLine1", order.ShippingAddressLine1));
            orderCommand.Parameters.Add(new SqlParameter("@ShippingCity", order.ShippingCity));
            orderCommand.Parameters.Add(new SqlParameter("@ShippingPostalCode", order.ShippingPostalCode));
            orderCommand.Parameters.Add(new SqlParameter("@ShippingCountry", order.ShippingCountry));
            orderCommand.Parameters.Add(new SqlParameter("@TotalAmount", order.TotalAmount));

            await using var reader = await orderCommand.ExecuteReaderAsync(cancellationToken);
            await reader.ReadAsync(cancellationToken);

            var createdOrder = new OrderCreated
            {
                Id = reader.GetInt32(0),
                TotalAmount = reader.GetDecimal(1),
                CreatedAtUtc = reader.GetDateTime(2)
            };

            await reader.CloseAsync();

            foreach (var item in order.Items)
            {
                await using var itemCommand = connection.CreateCommand();
                itemCommand.Transaction = (SqlTransaction)transaction;
                itemCommand.CommandText = itemSql;
                itemCommand.Parameters.Add(new SqlParameter("@OrderId", createdOrder.Id));
                itemCommand.Parameters.Add(new SqlParameter("@ProductId", item.ProductId));
                itemCommand.Parameters.Add(new SqlParameter("@ProductName", item.ProductName));
                itemCommand.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                itemCommand.Parameters.Add(new SqlParameter("@UnitPrice", item.UnitPrice));
                itemCommand.Parameters.Add(new SqlParameter("@LineTotal", item.LineTotal));

                await itemCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return createdOrder;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
