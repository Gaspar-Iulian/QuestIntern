namespace Ecommerce.Api.Services;

public interface IPasswordHasher
{
    (string Hash, string Salt) Hash(string password);
    bool Verify(string password, string expectedHash, string salt);
}
