using IdentityWebApi.ApplicationLogic.Services.User.Models;
using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user CQRS command.
/// </summary>
public record AuthenticateUserCommand : IRequest<ServiceResult<AuthUserResponse>>
{
    /// <summary>
    /// Gets user email.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets user password.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateUserCommand"/> class.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    public AuthenticateUserCommand(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
}