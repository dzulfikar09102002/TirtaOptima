using System.Security.Cryptography;
using System.Text;

namespace TirtaOptima.Helpers
{
    public class EncryptHelper
    {
        private static readonly byte[] KE = GenerateAesKey();

        public static string GeneratedPassword(string user, string pass)
        {
            using (TripleDES des = TripleDES.Create())
            using (MD5 md5 = MD5.Create())
            {
                byte[] userBytes = Encoding.UTF8.GetBytes(user.ToUpper().Trim());
                byte[] keyHash = md5.ComputeHash(userBytes);

                byte[] key = new byte[24];
                Array.Copy(keyHash, 0, key, 0, keyHash.Length);
                Array.Copy(keyHash, 0, key, keyHash.Length, 8);

                des.Key = key;
                des.Mode = CipherMode.ECB;

                byte[] buffer = Encoding.UTF8.GetBytes(pass);
                ICryptoTransform encryptor = des.CreateEncryptor();

                byte[] encrypted = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                return Convert.ToBase64String(encrypted);
            }
        }

        public static string EncryptToBase64(string originalText)
        {
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(originalText);
            byte[] encryptedBytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = KE;
                aes.GenerateIV();

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptToBase64(string et)
        {
            byte[] encryptedBytes = Convert.FromBase64String(et);
            string decryptedText = "";

            using (Aes aes = Aes.Create())
            {
                aes.Key = KE;

                using (MemoryStream ms = new MemoryStream(encryptedBytes))
                {
                    byte[] iv = new byte[16];
                    ms.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        decryptedText = sr.ReadToEnd();
                    }
                }
            }

            return decryptedText;
        }

        public static string EncryptString(string text, string keyString)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(text);
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(
                    keyString,
                    new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 },
                    10000,
                    HashAlgorithmName.SHA256))
                {
                    aes.Key = pdb.GetBytes(32);
                    aes.IV = pdb.GetBytes(16);
                }

                aes.Mode = CipherMode.CBC;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            string decryptedText = "";

            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(
                    keyString,
                    new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 },
                    10000,
                    HashAlgorithmName.SHA256))
                {
                    aes.Key = pdb.GetBytes(32);
                    aes.IV = pdb.GetBytes(16);
                }

                aes.Mode = CipherMode.CBC;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    decryptedText = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            return decryptedText;
        }

        public static DateTime GetSysdate(System.Data.Common.DbConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT GETDATE()";

                object? obj = command.ExecuteScalar();
                return Convert.ToDateTime(obj);
            }
        }

        public static string GetRandomKey(int length = 6)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            char[] chars = new char[length];
            byte[] data = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[data[i] % validChars.Length];
            }

            return new string(chars);
        }

        private static byte[] GenerateAesKey()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string keySource = "your-secure-key-source";
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(keySource));
            }
        }
    }
}
