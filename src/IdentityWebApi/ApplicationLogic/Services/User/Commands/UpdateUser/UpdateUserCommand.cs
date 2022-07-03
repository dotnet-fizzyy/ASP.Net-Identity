using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.UpdateUser;

/// <summary>
/// Update user CQRS command.
/// </summary>
public record UpdateUserCommand : IRequest<ServiceResult<UserResultDto>>
{
    /// <summary>
    /// Gets user.
    /// </summary>
    public UserDto User { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommand"/> class.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    public UpdateUserCommand(UserDto user)
    {
        this.User = user;
    }
}