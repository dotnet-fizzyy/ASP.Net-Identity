using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Startup.Settings
{
    public class AppSettings : IValidatable
    {
        public DbSettings DbSettings { get; set; }
        
        public SmtpClientSettings SmtpClientSettings { get; set; }
        
        public IdentitySettings IdentitySettings { get; set; }
        
        public void Validate()
        {
            DbSettings.Validate();
            SmtpClientSettings.Validate();
            IdentitySettings.Validate();
        }
    }
}