namespace Ecommerce.Api.Models;

public sealed class User
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public string PasswordSalt { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
}
