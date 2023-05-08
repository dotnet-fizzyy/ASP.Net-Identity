namespace IdentityWebApi.Startup.ApplicationSettings;

/// <summary>
/// JWT token configuration settings.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether JWT issuer should be validated.
    /// </summary>
    public bool ValidateIssuer { get; set; }

    /// <summary>
    /// Gets or sets JWT valid issuer.
    /// </summary>
    public string ValidIssuer { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether JWT audience should be validated.
    /// </summary>
    public bool ValidateAudience { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether JWT lifetime should be validated.
    /// </summary>
    public bool ValidateLifeTime { get; set; }

    /// <summary>
    /// Gets or sets JWT valid audience.
    /// </summary>
    public string ValidAudience { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether JWT issue signing key should be validated.
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; }

    /// <summary>
    /// Gets or sets JWT issue signing key.
    /// </summary>
    public string IssuerSigningKey { get; set; }

    /// <summary>
    /// Gets or sets JWT expiration minutes.
    /// </summary>
    public int ExpirationMinutes { get; set; }
}
