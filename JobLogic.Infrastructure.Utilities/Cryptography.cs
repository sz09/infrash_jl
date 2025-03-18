using System;
using System.Security.Cryptography;
using System.Text;

namespace JobLogic.Infrastructure.Utilities
{
    public static class Cryptography
    {
        public static string Decrypt(string ciphertext, string decryptKey)
        {
            byte[] encryptedBytes = Convert.FromBase64String(ciphertext);
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(decryptKey);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
