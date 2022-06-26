using NetEscapades.Configuration.Validation;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IdentityWebApi.ApplicationSettings;

/// <summary>
/// Regions verification settings.
/// </summary>
public class RegionsVerificationSettings : IValidatable
{
    /// <summary>
    /// Gets or sets a value indicating whether enable verification functionality is enabled.
    /// </summary>
    public bool EnableVerification { get; set; }

    /// <summary>
    /// Gets or sets a collection of prohibited regions identifiers.
    /// </summary>
    public ICollection<string> ProhibitedRegions { get; set; } = new List<string>();

    /// <summary>
    /// Gets a value indicating whether enable verification functionality can be used in application.
    /// </summary>
    public bool AllowVerification => this.EnableVerification && this.ProhibitedRegions.Any();

    /// <summary>
    /// Validates <see cref="RegionsVerificationSettings"/> params.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}