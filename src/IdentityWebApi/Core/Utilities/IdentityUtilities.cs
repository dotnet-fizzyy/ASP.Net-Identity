using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;
using System.Linq;

namespace IdentityWebApi.Core.Utilities;

/// <summary>
/// Identity server utilities.
/// </summary>
public static class IdentityUtilities
{
    /// <summary>
    /// Concatenates all identity error messages into one string separated by comma.
    /// </summary>
    /// <param name="errors">Collection of <see cref="IdentityError"/>.</param>
    /// <returns>String with errors description separated by comma.</returns>
    public static string ConcatenateIdentityErrorMessages(IEnumerable<IdentityError> errors)
    {
        if (!errors.Any())
        {
            return string.Empty;
        }

        var errorDescriptions = errors.Select(error => error.Description);

        return string.Join(", ", errorDescriptions);
    }
}