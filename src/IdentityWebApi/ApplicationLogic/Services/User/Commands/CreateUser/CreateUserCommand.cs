using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;

/// <summary>
/// Create user CQRS command.
/// </summary>
public record CreateUserCommand : IRequest<ServiceResult<UserDto>>
{
    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets user name.
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
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets user role.
    /// </summary>
    public string UserRole { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether email confirmation should be processed immediately.
    /// </summary>
    public bool ConfirmEmailImmediately { get; set; }
}
