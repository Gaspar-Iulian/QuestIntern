namespace Ecommerce.Api.Models;

public sealed class OrderDraft
{
    public int? UserId { get; init; }
    public string ShippingFullName { get; init; } = string.Empty;
    public string ShippingAddressLine1 { get; init; } = string.Empty;
    public string ShippingCity { get; init; } = string.Empty;
    public string ShippingPostalCode { get; init; } = string.Empty;
    public string ShippingCountry { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public IReadOnlyList<OrderItemDraft> Items { get; init; } = [];
}
