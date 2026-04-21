namespace Ecommerce.Api.Responses;

public sealed class AuthResponse
{
    public AuthUserResponse User { get; init; } = new();
    public string Message { get; init; } = string.Empty;
}
