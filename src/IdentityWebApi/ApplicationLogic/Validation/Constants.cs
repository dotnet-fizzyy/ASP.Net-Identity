namespace IdentityWebApi.ApplicationLogic.Validation;

/// <summary>
/// Validation message constants.
/// </summary>
public static class ValidationConstants
{
    /// <summary>
    /// Error message for missing or empty value.
    /// </summary>
    public const string NullOrEmptyValue = "{PropertyName} could not be null or empty. Provided value: {PropertyValue}";
}