using System.Collections.Generic;


namespace Rapimesa.Data
{
    public class CategoryManager
    {
        public CategoryManager()
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Category(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL);";
                cmd.ExecuteNonQuery();
            }
        }

        public List<Category> GetCategories()
        {
            var list = new List<Category>();
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id,Name FROM Category";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return list;
        }

        public void AddCategory(Category category)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Category(Name) VALUES(@n);";
                cmd.Parameters.AddWithValue("@n", category.Name);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE Category SET Name=@n WHERE Id=@id";
                cmd.Parameters.AddWithValue("@n", category.Name);
                cmd.Parameters.AddWithValue("@id", category.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCategory(int id)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Category WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
