using Npgsql;

namespace DAL;

public static class DbConnection
{
    private static NpgsqlConnection? _connection;

    public static NpgsqlConnection GetConnection()
    {
        if (_connection == null)
        {
            var connectionString = "Host=localhost;Username=myuser;Password=mypassword;Database=mydatabase";
            _connection = new NpgsqlConnection(connectionString);
        }

        return _connection;
    }
}