namespace IdentityWebApi.Startup.Settings
{
    public class SmtpClientSettings
    {
        public string Host { get; set; }
        
        public int Port { get; set; }

        public string EmailName { get; set; }
        
        public string EmailAddress { get; set; }

        public string Password { get; set; }
        
        public bool UseSsl { get; set; }
    }
}