namespace IdentityWebApi.Core.Constants;

/// <summary>
/// Exception messages.
/// </summary>
public static class EmailSubjects
{
    /// <summary>
    /// Gets account confirmation subject template.
    /// </summary>
    public const string EmailConfirmation = "Account email confirmation";
}

/// <summary>
/// Email template names.
/// </summary>
public static class Templates
{
    /// <summary>
    /// Gets email confirmation template name.
    /// </summary>
    public const string EmailConfirmationTemplate = "EmailConfirmation";
}

/// <summary>
/// Exception messages.
/// </summary>
public static class ExceptionMessageConstants
{
    /// <summary>
    /// Gets missing user exception message.
    /// </summary>
    public const string MissingUser = "No such user exists";

    /// <summary>
    /// Gets invalid authentication data exception message.
    /// </summary>
    public const string InvalidAuthData = "Unable to find user with provided parameter";
}

/// <summary>
/// Exception messages.
/// </summary>
public static class UserRoleConstants
{
    /// <summary>
    /// Gets admin role name.
    /// </summary>
    public const string Admin = "Administrator";

    /// <summary>
    /// Gets user role name.
    /// </summary>
    public const string User = "User";
}

/// <summary>
/// Authentication and authorization constants.
/// </summary>
public static class AuthConstants
{
    /// <summary>
    /// Gets application authentication and authorization policy name.
    /// </summary>
    public const string AppAuthPolicyName = "IdentityAuthenticationPolicy";

    /// <summary>
    /// Gets cookies authentication and authorization scheme name.
    /// </summary>
    public const string CookiesAuthScheme = "Cookies";

    /// <summary>
    /// Gets JWT authentication and authorization scheme name.
    /// </summary>
    public const string JwtBearerAuthType = "Bearer";
}

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
