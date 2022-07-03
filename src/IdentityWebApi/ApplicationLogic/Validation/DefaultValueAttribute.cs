using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.ApplicationLogic.Validation;

/// <summary>
/// Validation of default allowed value.
/// </summary>
public class DefaultValueAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates if value does not match default type value.
    /// </summary>
    /// <param name="value">Value of property.</param>
    /// <returns>Result indicating whether value is valid or not.</returns>
    public override bool IsValid(object value) =>
        value switch
        {
            Guid guid => guid != default,
            string @string => !string.IsNullOrEmpty(@string),
            int @int => @int != default,
            var _ => true
        };
}
