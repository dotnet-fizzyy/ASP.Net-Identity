using IdentityWebApi.ApplicationLogic.Services.Common;
using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.SoftRemoveRoleById;

/// <summary>
/// Soft remove role by id CQRS command.
/// </summary>
public class SoftRemoveRoleByIdCommand : IBaseId, IRequest<ServiceResult>
{
    /// <summary>
    /// Gets role id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveRoleByIdCommand"/> class.
    /// </summary>
    /// <param name="id">Role id.</param>
    public SoftRemoveRoleByIdCommand(Guid id)
    {
        this.Id = id;
    }
}
