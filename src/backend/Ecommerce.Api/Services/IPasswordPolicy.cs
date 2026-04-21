namespace Ecommerce.Api.Services;

public interface IPasswordPolicy
{
    IReadOnlyList<string> Validate(string password);
}
