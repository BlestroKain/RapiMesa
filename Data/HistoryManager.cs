using System;
using System.Collections.Generic;


namespace Rapimesa.Data
{
    public class HistoryManager
    {
        public HistoryManager()
        {
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS History (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Fecha TEXT NOT NULL,
                    Usuario TEXT NOT NULL,
                    Producto TEXT NOT NULL,
                    Cantidad INTEGER NOT NULL,
                    PrecioUnidad REAL NOT NULL,
                    Total REAL NOT NULL,
                    Movimiento TEXT NOT NULL
                );";
            cmd.ExecuteNonQuery();
        }

        public static void Add(HistoryEntry entry)
        {
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO History(Fecha, Usuario, Producto, Cantidad, PrecioUnidad, Total, Movimiento)
                VALUES(@fecha, @usuario, @producto, @cantidad, @precio, @total, @mov);";
            cmd.Parameters.AddWithValue("@fecha", entry.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@usuario", entry.Usuario);
            cmd.Parameters.AddWithValue("@producto", entry.Producto);
            cmd.Parameters.AddWithValue("@cantidad", entry.Cantidad);
            cmd.Parameters.AddWithValue("@precio", entry.PrecioUnidad);
            cmd.Parameters.AddWithValue("@total", entry.Total);
            cmd.Parameters.AddWithValue("@mov", entry.Movimiento);
            cmd.ExecuteNonQuery();
        }

        public static List<HistoryEntry> GetAll()
        {
            var list = new List<HistoryEntry>();
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM History ORDER BY Fecha DESC";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new HistoryEntry
                {
                    Id = reader.GetInt32(0),
                    Fecha = DateTime.Parse(reader.GetString(1)),
                    Usuario = reader.GetString(2),
                    Producto = reader.GetString(3),
                    Cantidad = reader.GetInt32(4),
                    PrecioUnidad = reader.GetDouble(5),
                    Total = reader.GetDouble(6),
                    Movimiento = reader.GetString(7)
                });
            }
            return list;
        }

        public static List<HistoryEntry> GetByUser(string usuario)
        {
            var list = new List<HistoryEntry>();
            var conn = ConnectionManager.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM History WHERE Usuario = @user ORDER BY Fecha DESC";
            cmd.Parameters.AddWithValue("@user", usuario);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new HistoryEntry
                {
                    Id = reader.GetInt32(0),
                    Fecha = DateTime.Parse(reader.GetString(1)),
                    Usuario = reader.GetString(2),
                    Producto = reader.GetString(3),
                    Cantidad = reader.GetInt32(4),
                    PrecioUnidad = reader.GetDouble(5),
                    Total = reader.GetDouble(6),
                    Movimiento = reader.GetString(7)
                });
            }
            return list;
        }
        public static void Insert(HistoryEntry entry)
        {
            Add(entry);
        }

       
    }

    public class HistoryEntry
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Usuario { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public double PrecioUnidad { get; set; }
        public double Total { get; set; }
        public string Movimiento { get; set; } = string.Empty;
    }
}
