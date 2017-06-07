namespace Ehb.Dijlezonen.Kassa.Infrastructure.Authentication
{
    public class SecurePassword
    {
        public SecurePassword(string password, string salt)
        {
            Password = password;
            Salt = salt;
        }

        public string Password { get; }
        public string Salt { get; }
    }
}