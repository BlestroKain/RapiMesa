using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Rapimesa.Data
{
    public static class ProductManager
    {
        static ProductManager()
        {
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Product(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    Stock INTEGER NOT NULL DEFAULT 0,
                    PrecioUnidad REAL NOT NULL DEFAULT 0
                );";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Dispose();
        }

        public static List<Product> GetAll()
        {
            var list = new List<Product>();
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Stock, PrecioUnidad FROM Product;";
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Stock = reader.GetInt32(2),
                    PrecioUnidad = reader.GetDouble(3)
                });
            }

            reader.Dispose();
            cmd.Dispose();
            conn.Dispose();

            return list;
        }

        public static Product GetById(int id)
        {
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Stock, PrecioUnidad FROM Product WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();

            Product product = null;

            if (reader.Read())
            {
                product = new Product
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Stock = reader.GetInt32(2),
                    PrecioUnidad = reader.GetDouble(3)
                };
            }

            reader.Dispose();
            cmd.Dispose();
            conn.Dispose();

            return product;
        }
        public static int Add(Product product)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
            INSERT INTO Product (Nombre, Stock, PrecioUnidad)
            VALUES (@n, @s, @p);
            SELECT last_insert_rowid();";

                cmd.Parameters.AddWithValue("@n", product.Nombre);
                cmd.Parameters.AddWithValue("@s", product.Stock);
                cmd.Parameters.AddWithValue("@p", product.PrecioUnidad);

                // Obtener el ID generado automáticamente
                int id = Convert.ToInt32((long)cmd.ExecuteScalar());
                product.Id = id;
                return id;
            }
        }


        public static void Update(Product product)
        {
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Product
                SET Nombre = @n,
                    Stock = @s,
                    PrecioUnidad = @p
                WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@n", product.Nombre);
            cmd.Parameters.AddWithValue("@s", product.Stock);
            cmd.Parameters.AddWithValue("@p", product.PrecioUnidad);
            cmd.Parameters.AddWithValue("@id", product.Id);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
        }

        public static void UpdateStock(int id, int nuevoStock)
        {
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Product SET Stock = @s WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@s", nuevoStock);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
        }

        public static void Delete(int id)
        {
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Product WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public double PrecioUnidad { get; set; }
    }
}
