namespace IdentityWebApi.Startup.ApplicationSettings;

/// <summary>
/// Email settings.
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether email confirmation is required.
    /// </summary>
    public bool RequireConfirmation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether unique email is required.
    /// </summary>
    public bool RequiredUniqueEmail { get; set; }
}
