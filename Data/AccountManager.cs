using System.Collections.Generic;
using Mono.Data.Sqlite;

namespace Rapimesa.Data
{
    public class AccountManager
    {
        public AccountManager()
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Account(
                    Username TEXT PRIMARY KEY,
                    Password TEXT NOT NULL,
                    Rol TEXT NOT NULL,
                    NombreCompleto TEXT,
                    FailedAttempts INTEGER DEFAULT 0,
                    IsLocked INTEGER DEFAULT 0,
                    SecurityQuestion TEXT,
                    SecurityAnswer TEXT);";
                cmd.ExecuteNonQuery();
            }
        }

        public void AddUser(User user)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Account(Username,Password,Rol,NombreCompleto,FailedAttempts,IsLocked,SecurityQuestion,SecurityAnswer) VALUES(@u,@p,@r,@n,@f,@l,@sq,@sa);";
                cmd.Parameters.AddWithValue("@u", user.Username);
                cmd.Parameters.AddWithValue("@p", user.Password);
                cmd.Parameters.AddWithValue("@r", user.Rol);
                cmd.Parameters.AddWithValue("@n", user.NombreCompleto);
                cmd.Parameters.AddWithValue("@f", user.FailedAttempts);
                cmd.Parameters.AddWithValue("@l", user.IsLocked ? 1 : 0);
                cmd.Parameters.AddWithValue("@sq", user.SecurityQuestion);
                cmd.Parameters.AddWithValue("@sa", user.SecurityAnswer);
                cmd.ExecuteNonQuery();
            }
        }

        public User GetUser(string username)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Username,Password,Rol,NombreCompleto,FailedAttempts,IsLocked,SecurityQuestion,SecurityAnswer FROM Account WHERE Username=@u";
                cmd.Parameters.AddWithValue("@u", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Username = reader.GetString(0),
                            Password = reader.GetString(1),
                            Rol = reader.GetString(2),
                            NombreCompleto = reader.IsDBNull(3) ? null : reader.GetString(3),
                            FailedAttempts = reader.GetInt32(4),
                            IsLocked = reader.GetInt32(5) == 1,
                            SecurityQuestion = reader.IsDBNull(6) ? null : reader.GetString(6),
                            SecurityAnswer = reader.IsDBNull(7) ? null : reader.GetString(7)
                        };
                    }
                }
            }
            return null;
        }

        public List<User> GetUsers()
        {
            var list = new List<User>();
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Username,Password,Rol,NombreCompleto,FailedAttempts,IsLocked,SecurityQuestion,SecurityAnswer FROM Account";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new User
                        {
                            Username = reader.GetString(0),
                            Password = reader.GetString(1),
                            Rol = reader.GetString(2),
                            NombreCompleto = reader.IsDBNull(3) ? null : reader.GetString(3),
                            FailedAttempts = reader.GetInt32(4),
                            IsLocked = reader.GetInt32(5) == 1,
                            SecurityQuestion = reader.IsDBNull(6) ? null : reader.GetString(6),
                            SecurityAnswer = reader.IsDBNull(7) ? null : reader.GetString(7)
                        });
                    }
                }
            }
            return list;
        }

        public void UpdateUser(User user)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE Account SET Password=@p,Rol=@r,NombreCompleto=@n,FailedAttempts=@f,IsLocked=@l,SecurityQuestion=@sq,SecurityAnswer=@sa WHERE Username=@u";
                cmd.Parameters.AddWithValue("@p", user.Password);
                cmd.Parameters.AddWithValue("@r", user.Rol);
                cmd.Parameters.AddWithValue("@n", user.NombreCompleto);
                cmd.Parameters.AddWithValue("@f", user.FailedAttempts);
                cmd.Parameters.AddWithValue("@l", user.IsLocked ? 1 : 0);
                cmd.Parameters.AddWithValue("@sq", user.SecurityQuestion);
                cmd.Parameters.AddWithValue("@sa", user.SecurityAnswer);
                cmd.Parameters.AddWithValue("@u", user.Username);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(string username)
        {
            using (var conn = ConnectionManager.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Account WHERE Username=@u";
                cmd.Parameters.AddWithValue("@u", username);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
