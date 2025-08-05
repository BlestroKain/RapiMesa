using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Rapimesa
{
    public static class UserFileManager
    {
        private static readonly string userFile = "usuarios.json";

        public static List<User> LoadUsers()
        {
            if (!File.Exists(userFile))
                return new List<User>();

            var cifrado = File.ReadAllText(userFile);
            if (string.IsNullOrWhiteSpace(cifrado))
                return new List<User>();

            try
            {
                var json = CryptoUtils.Desencriptar(cifrado);
                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        public static void SaveUsers(List<User> users)
        {
            var json = new JavaScriptSerializer().Serialize(users);
            var cifrado = CryptoUtils.Encriptar(json);
            File.WriteAllText(userFile, cifrado);
        }
    }
}
