namespace IdentityWebApi.Startup.Settings
{
    public class CookiesSettings
    {
        public bool SlidingExpiration { get; set; }
        
        public int ExpirationMinutes { get; set; }
    }
}