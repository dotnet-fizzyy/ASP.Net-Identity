using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.SoftRemoveRoleById;

/// <summary>
/// Soft remove role by id CQRS command.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SoftRemoveRoleByIdCommand"/> class.
/// </remarks>
/// <param name="id">Role id.</param>
public class SoftRemoveRoleByIdCommand(Guid id) : IBaseId, IRequest<ServiceResult>
{
    /// <summary>
    /// Gets role id.
    /// </summary>
    public Guid Id { get; } = id;
}
