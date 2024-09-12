using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Songcash.Configuration;
using Songcash.Model;

namespace Songcash.Repository;

public class UserRepository
{
    private string _connectionString;

    public UserRepository(IOptions<DatabaseConfiguration> options)
    {
        _connectionString = options.Value.ConnectionString;
    }


    public async Task<User> GetUserByEmail(string email)
    {
        var connection = new MySqlConnection(_connectionString);

        try
        {
            await connection.OpenAsync();

            var user = await connection.QueryAsync<User>("SELECT Id, Type, Name, Email FROM Users WHERE Email = @Email LIMIT 1 ", new { Email = email });
            if (!user.Any())
            {
                throw new ArgumentException("Request cannot be processed because User is incorrect");
            }

            return user.First();
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<int> InsertAndGetUserId(string email)
    {
        var connection = new MySqlConnection(_connectionString);

        try
        {
            await connection.OpenAsync();

            var result = await connection.QueryAsync<User>("SELECT Email FROM Users WHERE Email = @Email LIMIT 1 ", new { Email = email });
            if (!result.Any())
            {
                var insertQuery = @"INSERT INTO Users (Type, Name, Email) VALUES (@Type, @Name, @Email)";
                _ = await connection.ExecuteAsync(insertQuery,
                    new
                    {
                        Type = 1,
                        Name = "Sin nombre",
                        Email = email
                    });

                return await connection.GetInsertedId();
            }

            return result.First()!.Id;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}
