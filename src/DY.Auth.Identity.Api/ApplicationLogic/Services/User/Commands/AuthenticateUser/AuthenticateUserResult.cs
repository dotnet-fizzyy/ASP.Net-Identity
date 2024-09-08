namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user result model.
/// </summary>
public record AuthenticateUserResult
{
    /// <summary>
    /// Gets access token.
    /// </summary>
    public string AccessToken { get; init; }

    /// <summary>
    /// Gets authenticated user.
    /// </summary>
    public AuthUser User { get; init; }

    /// <summary>
    /// Authenticated user result model.
    /// </summary>
    public class AuthUser
    {
        /// <summary>
        /// Gets User Id.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets user phone.
        /// </summary>
        public string Phone { get; init; }

        /// <summary>
        /// Gets user email.
        /// </summary>
        public string Email { get; init; }
    }
}
