using System;
using System.Security.Cryptography;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Authentication
{
    public class Crypto
    {
        private const int Iterations = 10000;
        private const int SaltLength = 16;

        public string Encrypt(string password)
        {
            var salt = GetNewRandomSalt();

            return SecurePassword(password, salt);
        }

        private byte[] GetNewRandomSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[SaltLength];

                rng.GetBytes(salt);

                return salt;
            }
        }

        private string SecurePassword(string password, byte[] salt)
        {
            var hash = HashPasswordWithSalt(password, salt);

            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, SaltLength);
            Array.Copy(hash, 0, hashBytes, SaltLength, 20);

            var securePassword = Convert.ToBase64String(hashBytes);

            return securePassword;
        }

        private byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(20);

            return hash;
        }

        public bool Verify(string securePassword, string plainTextPassword)
        {
            var hashBytes = Convert.FromBase64String(securePassword);

            var salt = new byte[SaltLength];
            Array.Copy(hashBytes, 0, salt, 0, SaltLength);

            var hash = HashPasswordWithSalt(plainTextPassword, salt);

            for (var i = 0; i < 20; i++)
                if (hashBytes[i + SaltLength] != hash[i])
                    return false;

            return true;
        }
    }
}