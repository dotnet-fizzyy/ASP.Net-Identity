using DY.Auth.Identity.Api.ApplicationLogic.Shared.Requests;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.SoftRemoveRoleById;

/// <summary>
/// Soft remove role by id CQRS command.
/// </summary>
/// <param name="id">Role id.</param>
public class SoftRemoveRoleByIdCommand(Guid id) : IBaseId, IRequest<ServiceResult>
{
    /// <summary>
    /// Gets role id.
    /// </summary>
    public Guid Id { get; } = id;
}
