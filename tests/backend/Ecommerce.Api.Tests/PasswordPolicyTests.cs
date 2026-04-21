using Ecommerce.Api.Services;

namespace Ecommerce.Api.Tests;

public sealed class PasswordPolicyTests
{
    private readonly PasswordPolicy passwordPolicy = new();

    [Fact]
    public void Validate_WhenPasswordMeetsPolicy_ReturnsNoErrors()
    {
        var errors = passwordPolicy.Validate("DemoPass1@");

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WhenPasswordIsWeak_ReturnsPolicyErrors()
    {
        var errors = passwordPolicy.Validate("abc");

        Assert.Contains("Password must be at least 8 characters long.", errors);
        Assert.Contains("Password must contain at least one uppercase letter.", errors);
        Assert.Contains("Password must contain at least one number.", errors);
        Assert.Contains("Password must contain at least one special character.", errors);
    }
}
