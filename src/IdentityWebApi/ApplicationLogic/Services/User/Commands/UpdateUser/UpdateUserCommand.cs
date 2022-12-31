using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.UpdateUser;

/// <summary>
/// Update user CQRS command.
/// </summary>
public record UpdateUserCommand : IRequest<ServiceResult<UserResult>>
{
    /// <summary>
    /// Gets user.
    /// </summary>
    public Models.Action.UserDto UserDto { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommand"/> class.
    /// </summary>
    /// <param name="userDto"><see cref="UserDto"/>.</param>
    public UpdateUserCommand(Models.Action.UserDto userDto)
    {
        this.UserDto = userDto;
    }
}