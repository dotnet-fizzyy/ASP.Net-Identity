using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.HardRemoveRoleById;

/// <summary>
/// Hard remove role by id command CQRS command.
/// </summary>
public record HardRemoveRoleByIdCommand : IBaseId, IRequest<ServiceResult>
{
    /// <summary>
    /// Gets role id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveRoleByIdCommand"/> class.
    /// </summary>
    /// <param name="id">Role id.</param>
    public HardRemoveRoleByIdCommand(Guid id)
    {
        this.Id = id;
    }
}
