using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.CreateUser;

/// <summary>
/// Create user result model.
/// </summary>
public record CreateUserResult
{
    /// <summary>
    /// Gets user ID.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets username.
    /// </summary>
    public string UserName { get; init; }

    /// <summary>
    /// Gets concurrency stamp.
    /// </summary>
    public string ConcurrencyStamp { get; init; }

    /// <summary>
    /// Gets email.
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Gets phone number.
    /// </summary>
    public string PhoneNumber { get; init; }

    /// <summary>
    /// Gets password.
    /// </summary>
    public string Password { get; init; }

    /// <summary>
    /// Gets created and assigned user role.
    /// </summary>
    public UserRoleResult UserRole { get; init; }

    /// <summary>
    /// Created and assigned user role model.
    /// </summary>
    public record UserRoleResult
    {
        /// <summary>
        /// Gets role ID.
        /// </summary>
        public Guid RoleId { get; init; }

        /// <summary>
        /// Gets role name.
        /// </summary>
        public string Name { get; init; }
    }
}
