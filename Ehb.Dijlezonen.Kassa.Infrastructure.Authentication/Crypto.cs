using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Authentication
{
    public class Crypto
    {
        public SecurePassword Encrypt(string password)
        {
            var salt = GetNewRandomSalt();

            using (var hashing = SHA256.Create())
            {
                var passwordAsBytes = Encoding.Unicode.GetBytes(password);

                var hash = hashing.ComputeHash(passwordAsBytes.Concat(salt).ToArray());
                
                return new SecurePassword(
                    Encoding.Unicode.GetString(hash),
                    Encoding.Unicode.GetString(salt)
                );
            }
        }

        public bool Verify(SecurePassword password, string plainTextPassword)
        {
            using (var hashing = SHA256.Create())
            {
                var salt = Encoding.Unicode.GetBytes(password.Salt);
                var passwordAsBytes = Encoding.Unicode.GetBytes(plainTextPassword);

                var hash = hashing.ComputeHash(passwordAsBytes.Concat(salt).ToArray());

                return hash.SequenceEqual(Encoding.Unicode.GetBytes(password.Password));
            }
        }
        
        private static byte[] GetNewRandomSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                const int maxLength = 32;
                var salt = new byte[maxLength];

                rng.GetBytes(salt);

                return salt;
            }
        }
    }
}
