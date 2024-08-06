using DY.Auth.Identity.Api.ApplicationLogic.Models.Common;

using System.Collections.Generic;

namespace DY.Auth.Identity.Api.ApplicationLogic.Models.Output;

/// <summary>
/// Result "User" model.
/// </summary>
public class UserResult : BaseUser
{
    /// <summary>
    /// Gets or sets user roles names.
    /// </summary>
    public IEnumerable<string> Roles { get; set; }
}
