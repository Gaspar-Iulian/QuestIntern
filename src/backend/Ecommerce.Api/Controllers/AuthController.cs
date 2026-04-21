using Ecommerce.Api.Requests;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var (response, errors) = await authService.RegisterAsync(request, cancellationToken);

        if (response is null)
        {
            return BadRequest(new ValidationErrorResponse
            {
                Message = "Registration failed.",
                Errors = errors
            });
        }

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);

        if (response is null)
        {
            return Unauthorized(new ValidationErrorResponse
            {
                Message = "Invalid email or password.",
                Errors = ["Invalid email or password."]
            });
        }

        return Ok(response);
    }
}
