using Npgsql;

namespace dokotera.Models.connection
{
    public class Connect
    {
        private static string _connString = "Server=localhost;Port=5433;Username=postgres;Password=root;Database=dokotera";
        private static NpgsqlConnection? _conn;
        public static NpgsqlConnection? GetConnection()
        {
            try
            {
                _conn = new NpgsqlConnection(_connString);
                _conn.Open();
            }
            catch (Exception ex)
            {
                _conn = null;
            }

            return _conn;
        }
    }
}
