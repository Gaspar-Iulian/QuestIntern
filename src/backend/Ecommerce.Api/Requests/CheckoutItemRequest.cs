using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Requests;

public sealed class CheckoutItemRequest
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }

    [Range(1, 100)]
    public int Quantity { get; init; }
}
