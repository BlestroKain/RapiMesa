using Mono.Data.Sqlite;

namespace Rapimesa.Data
{
    public class StockManager
    {
        public StockManager()
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS History(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductId INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Type TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    User TEXT,
                    FOREIGN KEY(ProductId) REFERENCES Product(Id));";
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertHistory(int productId, int quantity, string type, string user)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO History(ProductId,Quantity,Type,User) VALUES(@p,@q,@t,@u);";
                cmd.Parameters.AddWithValue("@p", productId);
                cmd.Parameters.AddWithValue("@q", quantity);
                cmd.Parameters.AddWithValue("@t", type);
                cmd.Parameters.AddWithValue("@u", user);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
