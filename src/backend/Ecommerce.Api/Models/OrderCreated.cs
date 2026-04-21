namespace Ecommerce.Api.Models;

public sealed class OrderCreated
{
    public int Id { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
