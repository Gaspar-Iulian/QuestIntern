using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User> CreateAsync(string fullName, string email, string passwordHash, string passwordSalt, CancellationToken cancellationToken);
}
