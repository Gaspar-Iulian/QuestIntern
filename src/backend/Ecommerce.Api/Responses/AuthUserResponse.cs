namespace Ecommerce.Api.Responses;

public sealed class AuthUserResponse
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
