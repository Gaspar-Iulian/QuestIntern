using Ecommerce.Api.Models;
using Microsoft.Data.SqlClient;

namespace Ecommerce.Api.Repositories;

public sealed class UserRepository(SqlConnectionFactory connectionFactory) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, FullName, Email, PasswordHash, PasswordSalt, CreatedAtUtc
            FROM dbo.Users
            WHERE Email = @Email;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.Add(new SqlParameter("@Email", email));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapUser(reader);
    }

    public async Task<User> CreateAsync(
        string fullName,
        string email,
        string passwordHash,
        string passwordSalt,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Users (FullName, Email, PasswordHash, PasswordSalt)
            OUTPUT inserted.Id, inserted.FullName, inserted.Email, inserted.PasswordHash, inserted.PasswordSalt, inserted.CreatedAtUtc
            VALUES (@FullName, @Email, @PasswordHash, @PasswordSalt);
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.Add(new SqlParameter("@FullName", fullName));
        command.Parameters.Add(new SqlParameter("@Email", email));
        command.Parameters.Add(new SqlParameter("@PasswordHash", passwordHash));
        command.Parameters.Add(new SqlParameter("@PasswordSalt", passwordSalt));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);

        return MapUser(reader);
    }

    private static User MapUser(SqlDataReader reader)
    {
        return new User
        {
            Id = reader.GetInt32(0),
            FullName = reader.GetString(1),
            Email = reader.GetString(2),
            PasswordHash = reader.GetString(3),
            PasswordSalt = reader.GetString(4),
            CreatedAtUtc = reader.GetDateTime(5)
        };
    }
}
