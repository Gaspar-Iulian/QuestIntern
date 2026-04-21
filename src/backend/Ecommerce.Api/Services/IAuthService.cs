using Ecommerce.Api.Requests;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface IAuthService
{
    Task<(AuthResponse? Response, IReadOnlyList<string> Errors)> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
