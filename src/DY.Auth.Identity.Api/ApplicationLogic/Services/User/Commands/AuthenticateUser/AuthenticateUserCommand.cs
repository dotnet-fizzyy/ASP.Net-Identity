using DY.Auth.Identity.Api.ApplicationLogic.Models.Output;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user CQRS command.
/// </summary>
public record AuthenticateUserCommand : IRequest<ServiceResult<AuthUserResult>>
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
