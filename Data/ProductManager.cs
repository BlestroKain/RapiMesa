using System.Collections.Generic;
using Mono.Data.Sqlite;

namespace Rapimesa.Data
{
    public class ProductManager
    {
        public ProductManager()
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Product(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    CategoryId INTEGER,
                    Stock INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(CategoryId) REFERENCES Category(Id));";
                cmd.ExecuteNonQuery();
            }
        }

        public List<Product> GetProducts()
        {
            var list = new List<Product>();
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id,Name,CategoryId,Stock FROM Product";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            CategoryId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            Stock = reader.GetInt32(3)
                        });
                    }
                }
            }
            return list;
        }

        public void AddProduct(Product product)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Product(Name,CategoryId,Stock) VALUES(@n,@c,@s);";
                cmd.Parameters.AddWithValue("@n", product.Name);
                cmd.Parameters.AddWithValue("@c", product.CategoryId);
                cmd.Parameters.AddWithValue("@s", product.Stock);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE Product SET Name=@n,CategoryId=@c,Stock=@s WHERE Id=@id";
                cmd.Parameters.AddWithValue("@n", product.Name);
                cmd.Parameters.AddWithValue("@c", product.CategoryId);
                cmd.Parameters.AddWithValue("@s", product.Stock);
                cmd.Parameters.AddWithValue("@id", product.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int id)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Product WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public int Stock { get; set; }
    }
}
