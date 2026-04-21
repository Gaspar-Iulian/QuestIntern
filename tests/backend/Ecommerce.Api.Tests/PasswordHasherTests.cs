using Ecommerce.Api.Services;

namespace Ecommerce.Api.Tests;

public sealed class PasswordHasherTests
{
    private readonly PasswordHasher passwordHasher = new();

    [Fact]
    public void Verify_WhenPasswordMatchesHash_ReturnsTrue()
    {
        var password = "DemoPass1@";
        var hashedPassword = passwordHasher.Hash(password);

        var isValid = passwordHasher.Verify(password, hashedPassword.Hash, hashedPassword.Salt);

        Assert.True(isValid);
    }

    [Fact]
    public void Verify_WhenPasswordDoesNotMatchHash_ReturnsFalse()
    {
        var hashedPassword = passwordHasher.Hash("DemoPass1@");

        var isValid = passwordHasher.Verify("WrongPass1@", hashedPassword.Hash, hashedPassword.Salt);

        Assert.False(isValid);
    }
}
