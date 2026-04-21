using Ecommerce.Api.Requests;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface ICheckoutService
{
    Task<(CheckoutResponse? Response, IReadOnlyList<string> Errors)> PlaceOrderAsync(CheckoutRequest request, CancellationToken cancellationToken);
}
