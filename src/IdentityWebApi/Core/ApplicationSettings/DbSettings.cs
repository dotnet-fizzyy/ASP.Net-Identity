using IdentityWebApi.ApplicationLogic.Validation;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Core.ApplicationSettings;

/// <summary>
/// Database settings.
/// </summary>
public class DbSettings : IValidatable
{
    /// <summary>
    /// Gets or sets DB host.
    /// </summary>
    [Required]
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets DB Port.
    /// </summary>
    [DefaultValue]
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets DB Instance.
    /// </summary>
    [Required]
    public string Instance { get; set; }

    /// <summary>
    /// Gets or sets DB User.
    /// </summary>
    [Required]
    public string User { get; set; }

    /// <summary>
    /// Gets or sets DB Password.
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Gets computed DB connection string.
    /// </summary>
    public string ConnectionString =>
        $"Server={this.Host},{this.Port};" +
        $"Database={this.Instance};" +
        $"User={this.User};" +
        $"Password={this.Password};" +
        "MultipleActiveResultSets=True;" +
        "TrustServerCertificate=True;";

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
