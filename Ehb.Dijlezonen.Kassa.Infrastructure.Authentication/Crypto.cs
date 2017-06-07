using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Authentication
{
    public class Crypto
    {
        private readonly Encoding encoding = Encoding.Unicode;

        public SecurePassword Encrypt(string password)
        {
            var salt = GetNewRandomSalt();

            return SecurePassword(password, salt);
        }

        private SecurePassword SecurePassword(string password, byte[] salt)
        {
            using (var hashing = SHA256.Create())
            {
                var passwordAsBytes = encoding.GetBytes(password);

                var hash = hashing.ComputeHash(passwordAsBytes.Concat(salt).ToArray());

                return new SecurePassword(
                    encoding.GetString(hash),
                    encoding.GetString(salt)
                );
            }
        }

        public bool Verify(SecurePassword secure, string plainTextPassword)
        {
            var salt = encoding.GetBytes(secure.Salt);

            var password = SecurePassword(plainTextPassword, salt);
            return password.Password == secure.Password;
        }
        
        private byte[] GetNewRandomSalt()
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
