namespace Ecommerce.Api.Services;

public sealed class PasswordPolicy : IPasswordPolicy
{
    public const int MinimumLength = 8;

    public IReadOnlyList<string> Validate(string password)
    {
        var errors = new List<string>();

        if (password.Length < MinimumLength)
        {
            errors.Add($"Password must be at least {MinimumLength} characters long.");
        }

        if (!password.Any(char.IsUpper))
        {
            errors.Add("Password must contain at least one uppercase letter.");
        }

        if (!password.Any(char.IsLower))
        {
            errors.Add("Password must contain at least one lowercase letter.");
        }

        if (!password.Any(char.IsDigit))
        {
            errors.Add("Password must contain at least one number.");
        }

        if (!password.Any(IsSpecialCharacter))
        {
            errors.Add("Password must contain at least one special character.");
        }

        return errors;
    }

    private static bool IsSpecialCharacter(char value)
    {
        return !char.IsLetterOrDigit(value);
    }
}
