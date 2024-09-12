using MySql.Data.MySqlClient;

namespace Songcash.Repository;
public static class RepositoryExtensions
{
    public static async Task<int> GetInsertedId(this MySqlConnection connection)
    {
        var insertedIdQuery = "SELECT LAST_INSERT_ID();";
        using var command = new MySqlCommand(insertedIdQuery, connection);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }
}