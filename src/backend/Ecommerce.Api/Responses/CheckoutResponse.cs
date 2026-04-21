namespace Ecommerce.Api.Responses;

public sealed class CheckoutResponse
{
    public int OrderId { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public IReadOnlyList<CheckoutItemResponse> Items { get; init; } = [];
    public string Message { get; init; } = string.Empty;
}
