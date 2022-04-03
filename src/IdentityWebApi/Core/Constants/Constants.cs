namespace IdentityWebApi.Core.Constants;

public static class Templates
{
    public const string EmailConfirmationTemplate = "EmailConfirmation";
}

public static class ExceptionMessageConstants
{
    public const string MissingUser = "No such user exists";
    public const string InvalidAuthData = "Unable to find user with provided parameter";
}

public static class EmailSubjects
{
    public const string AccountConfirmation = "Account confirmation";
}

public static class UserRoleConstants
{
    public const string Admin = "Administrator";
    public const string User = "User";
}
