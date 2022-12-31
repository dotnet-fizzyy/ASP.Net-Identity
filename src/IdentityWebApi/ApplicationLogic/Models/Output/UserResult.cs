using System.Collections.Generic;

using IdentityWebApi.ApplicationLogic.Models.Common;

namespace IdentityWebApi.ApplicationLogic.Models.Output;

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