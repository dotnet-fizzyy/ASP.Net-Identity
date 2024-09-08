using DY.Auth.Identity.Api.Core.Results;

using MediatR;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user CQRS command.
/// </summary>
public record AuthenticateUserCommand : IRequest<ServiceResult<AuthenticateUserResult>>
{
    /// <summary>
    /// Gets user email.
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Gets user password.
    /// </summary>
    public string Password { get; init; }
}
