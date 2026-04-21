using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories;
using Ecommerce.Api.Requests;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IPasswordPolicy passwordPolicy) : IAuthService
{
    public async Task<(AuthResponse? Response, IReadOnlyList<string> Errors)> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var fullName = request.FullName.Trim();
        var errors = passwordPolicy.Validate(request.Password).ToList();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            errors.Add("Full name is required.");
        }

        var existingUser = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (existingUser is not null)
        {
            errors.Add("An account with this email already exists.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var password = passwordHasher.Hash(request.Password);
        var user = await userRepository.CreateAsync(fullName, email, password.Hash, password.Salt, cancellationToken);

        return (CreateResponse(user, "Account created successfully."), []);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return null;
        }

        return CreateResponse(user, "Logged in successfully.");
    }

    private static AuthResponse CreateResponse(User user, string message)
    {
        return new AuthResponse
        {
            Message = message,
            User = new AuthUserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            }
        };
    }
}
