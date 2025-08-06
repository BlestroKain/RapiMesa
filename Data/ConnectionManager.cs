using Microsoft.Data.Sqlite;

namespace Rapimesa.Data
{
    public static class ConnectionManager
    {
        private const string ConnectionString = "Data Source=rapimesa.db;";

        public static SqliteConnection GetConnection()
        {
            var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
