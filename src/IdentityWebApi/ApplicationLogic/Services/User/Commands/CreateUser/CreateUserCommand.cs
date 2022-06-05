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
    /// Gets user name.
    /// </summary>
    public UserDto User { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    public CreateUserCommand(UserDto user)
    {
        this.User = user;
    }
}