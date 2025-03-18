using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Utilities
{
    public static class EncryptionUtils
    {
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        [Obsolete("Should use Async version instead")]
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var result = string.Empty;
            using (var rfc = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)))
            {
                byte[] keyBytes = rfc.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
                {
                    var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                    byte[] cipherTextBytes;

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memoryStream.ToArray();
                        }
                    }

                    result = Convert.ToBase64String(cipherTextBytes);
                }
            }

            return result;
        }

        public static async Task<string> EncryptAsync(string plainText, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var result = string.Empty;
            using (var rfc = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)))
            {
                byte[] keyBytes = rfc.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
                {
                    var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                    byte[] cipherTextBytes;

                    using (var memoryStream = new MemoryStream())
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        await cryptoStream.WriteAsync(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                    }

                    result = Convert.ToBase64String(cipherTextBytes);
                }
            }

            return result;
        }

        [Obsolete("Should use Async version instead")]
        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            var result = string.Empty;
            using (var rfc = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)))
            {
                byte[] keyBytes = rfc.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None })
                {
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey)))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<string> DecryptAsync(string encryptedText, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            var result = string.Empty;
            using (var rfc = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)))
            {
                byte[] keyBytes = rfc.GetBytes(256 / 8);

                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None })
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey)))
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                    int decryptedByteCount = await cryptoStream.ReadAsync(plainTextBytes, 0, plainTextBytes.Length);
                    result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                }
            }

            return result;
        }

        public static string CalculateMD5Hash(string input)
        {
            string result = string.Empty;

            // step 1, calculate MD5 hash from input
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                result = sb.ToString();
            }

            return result;
        }
    }
}
