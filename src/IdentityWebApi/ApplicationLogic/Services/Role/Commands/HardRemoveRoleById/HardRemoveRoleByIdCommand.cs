using IdentityWebApi.ApplicationLogic.Services.Common;
using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.HardRemoveRoleById;

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
