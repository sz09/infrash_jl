using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JobLogic.Infrastructure.Utilities
{
    public static class SecurityUtils
    {
        public static string EncodedPassword(string password, DateTime timestamp)
        {
            if (string.IsNullOrWhiteSpace(password)) return string.Empty;
            string salt = timestamp.ToString("yyyyMMddHHmmss");
            return EncodeString(password, salt);
        }

        private static string EncodeString(string value, string salt)
        {
            string result = string.Empty;

            using (HMACSHA1 hash = new HMACSHA1())
            {
                hash.Key = Encoding.Unicode.GetBytes(salt);
                result = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(value)));
            }

            return result;
        }

        public static object EncodedPassword(string password, object timestamp)
        {
            throw new NotImplementedException();
        }
        public static DateTime SetDateTimeFormat(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
        public static string EncryptString(string key, string plainText, bool needHashKey = true)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = needHashKey ? GetHash(key) : Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText, bool needHashKey = true)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = needHashKey ? GetHash(key) : Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static byte[] GetHash(string rawKey)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
        }
    }
}
