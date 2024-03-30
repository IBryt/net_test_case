using Npgsql;
using System.Data;

namespace WebAPI.Data;

public class PostgresDBConnectionFactory : IDbConnectionFactory
{
    private readonly string? _connectionString;

    public PostgresDBConnectionFactory(string? connectionString)
    {
        _connectionString = connectionString;
    }


    public async Task<IDbConnection> GetDbConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        return conn;
    }
}

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetDbConnection();
}
