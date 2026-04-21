using Ecommerce.Api.Requests;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/checkout")]
public sealed class CheckoutController(ICheckoutService checkoutService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CheckoutRequest request, CancellationToken cancellationToken)
    {
        var (response, errors) = await checkoutService.PlaceOrderAsync(request, cancellationToken);

        if (response is null)
        {
            return BadRequest(new ValidationErrorResponse
            {
                Message = "Checkout failed.",
                Errors = errors
            });
        }

        return StatusCode(StatusCodes.Status201Created, response);
    }
}
