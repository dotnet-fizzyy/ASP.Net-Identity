using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;

/// <summary>
/// Create user CQRS command.
/// </summary>
public record CreateUserCommand : IRequest<ServiceResult<UserDto>>
{
    /// <summary>
    /// Gets user.
    /// </summary>
    public UserDto User { get; }

    /// <summary>
    /// Gets a value indicating whether email confirmation should be processed immediately.
    /// </summary>
    public bool ConfirmEmailImmediately { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    /// <param name="confirmEmailImmediately">indicating whether email confirmation should be immediate.</param>
    public CreateUserCommand(UserDto user, bool confirmEmailImmediately)
    {
        this.User = user;
        this.ConfirmEmailImmediately = confirmEmailImmediately;
    }
}