using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rapimesa
{
    public static class CryptoUtils
    {
        private static readonly string clave = "tu_clave_secreta_1234"; // TODO: make secure

        public static string Encriptar(string texto)
        {
            byte[] key = Encoding.UTF8.GetBytes(clave.PadRight(32).Substring(0,32));
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, 16);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(texto);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Desencriptar(string cifrado)
        {
            byte[] data = Convert.FromBase64String(cifrado);
            byte[] key = Encoding.UTF8.GetBytes(clave.PadRight(32).Substring(0,32));
            using (var aes = Aes.Create())
            {
                byte[] iv = data.Take(16).ToArray();
                using (var decryptor = aes.CreateDecryptor(key, iv))
                using (var ms = new MemoryStream(data.Skip(16).ToArray()))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
