using IdentityWebApi.ApplicationLogic.Validation;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Startup.Settings;

public class DbSettings : IValidatable
{
    [Required] 
    public string Host { get; set; }

    [DefaultValue] 
    public int Port { get; set; }

    [Required] 
    public string Instance { get; set; }

    [Required] 
    public string User { get; set; }

    [Required] 
    public string Password { get; set; }

    public string ConnectionString =>
        $"Server={Host},{Port};Database={Instance};User={User};Password={Password};MultipleActiveResultSets=True;TrustServerCertificate=True;";

    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
