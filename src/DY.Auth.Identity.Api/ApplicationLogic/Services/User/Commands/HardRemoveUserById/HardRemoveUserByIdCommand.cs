using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.HardRemoveUserById;

/// <summary>
/// Hard remove user by id CQRS command.
/// </summary>
public record HardRemoveUserByIdCommand : IBaseId, IRequest<ServiceResult>
{
    /// <summary>
    /// Gets user id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveUserByIdCommand"/> class.
    /// </summary>
    /// <param name="id">User id.</param>
    public HardRemoveUserByIdCommand(Guid id)
    {
        this.Id = id;
    }
}
