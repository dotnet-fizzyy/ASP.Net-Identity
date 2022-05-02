using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Core.ApplicationSettings;

public class AppSettings : IValidatable
{
    public DbSettings DbSettings { get; set; }

    public SmtpClientSettings SmtpClientSettings { get; set; }

    public RegionsVerificationSettings RegionsVerificationSettings { get; set; }
    
    public IpStackSettings IpStackSettings { get; set; }
    
    public IdentitySettings IdentitySettings { get; set; }

    public void Validate()
    {
        DbSettings.Validate();
        SmtpClientSettings.Validate();
        IdentitySettings.Validate();
    }
}
