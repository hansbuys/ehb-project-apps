namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    public class TokenAuthenticationOptions
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string TokenPath { get; set; }
        public bool UseFakeCredentials { get; set; }
    }
}