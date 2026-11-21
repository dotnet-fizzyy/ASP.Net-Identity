using DY.Auth.Identity.Api.ApplicationLogic.Shared.Requests;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.RemoveRoleById;

/// <summary>
/// Remove role by id command CQRS command.
/// </summary>
public record RemoveRoleByIdCommand : IBaseId, IRequest<ServiceResult>
{
    /// <summary>
    /// Gets role id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveRoleByIdCommand"/> class.
    /// </summary>
    /// <param name="id">Role id.</param>
    public RemoveRoleByIdCommand(Guid id)
    {
        this.Id = id;
    }
}
