using NetEscapades.Configuration.Validation;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IdentityWebApi.Core.ApplicationSettings;

public class RegionsVerificationSettings : IValidatable
{
    public bool EnableVerification { get; set; }

    public ICollection<string> ProhibitedRegions { get; set; } = new List<string>();

    public bool AllowVerification => EnableVerification && ProhibitedRegions.Any();

    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}