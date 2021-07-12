namespace IdentityWebApi.BL.Constants
{
    public static class ExceptionMessageConstants
    {
        public const string MissingUser = "No such user exists";
        public const string InvalidAuthData = "Unable to find user with provided parameter";
    }

    public static class EmailSubjects
    {
        public const string AccountConfirmation = "Account confirmation";
    }
}