using System;
using System.Collections.Generic;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Queries.GetUserById;

/// <summary>
/// Get user by ID result model.
/// </summary>
public record GetUserByIdResult
{
    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets username.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets concurrency stamp.
    /// </summary>
    public string ConcurrencyStamp { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets phone number.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets assigned user roles.
    /// </summary>
    public ICollection<UserRoleResult> UserRoles { get; init; }

    /// <summary>
    /// Assigned user role model.
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
