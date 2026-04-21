using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Requests;

public sealed class RegisterUserRequest
{
    [Required]
    [MaxLength(120)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}
