namespace IdentityWebApi.Startup.Settings
{
    public class AppSettings
    {
        public string Url { get; set; }
        
        public DbSettings DbSettings { get; set; }
        
        public SmtpClientSettings SmtpClientSettings { get; set; }
        
        public IdentitySettings IdentitySettings { get; set; }
    }
}