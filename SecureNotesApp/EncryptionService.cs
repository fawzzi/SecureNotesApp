using System;
using System.IO;
using System.Security.Cryptography;

namespace SecureNotesApp
{
    public static class EncryptionService
    {
        private const int SaltSize = 16;
        private const int Iterations = 10000;

        private static byte[] DeriveKey(string password, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return deriveBytes.GetBytes(32);
            }
        }

        public static string Encrypt(string plainText, string password)
        {
            using (var aes = Aes.Create())
            {
                var salt = new byte[SaltSize];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                aes.Key = DeriveKey(password, salt);
                aes.GenerateIV();

                using (var ms = new MemoryStream())
                {
                    ms.Write(salt, 0, salt.Length);
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cryptoStream))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string password)
        {
            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);
                using (var aes = Aes.Create())
                {
                    var salt = new byte[SaltSize];
                    var iv = new byte[aes.BlockSize / 8];

                    Array.Copy(fullCipher, 0, salt, 0, SaltSize);
                    Array.Copy(fullCipher, SaltSize, iv, 0, iv.Length);

                    aes.Key = DeriveKey(password, salt);
                    aes.IV = iv;

                    using (var ms = new MemoryStream(fullCipher, SaltSize + iv.Length,
                               fullCipher.Length - (SaltSize + iv.Length)))

                    using (var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cryptoStream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}