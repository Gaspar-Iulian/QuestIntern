using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Requests;

public sealed class CheckoutRequest
{
    public int? UserId { get; init; }

    [Required]
    [MaxLength(120)]
    public string ShippingFullName { get; init; } = string.Empty;

    [Required]
    [MaxLength(240)]
    public string ShippingAddressLine1 { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string ShippingCity { get; init; } = string.Empty;

    [Required]
    [MaxLength(40)]
    public string ShippingPostalCode { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string ShippingCountry { get; init; } = string.Empty;

    [MinLength(1)]
    public IReadOnlyList<CheckoutItemRequest> Items { get; init; } = [];
}
