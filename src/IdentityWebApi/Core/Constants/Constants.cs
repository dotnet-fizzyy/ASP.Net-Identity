namespace IdentityWebApi.Core.Constants;

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
public static class EmailSubjects
{
    /// <summary>
    /// Gets account confirmation subject template.
    /// </summary>
    public const string AccountConfirmation = "Account confirmation";
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
