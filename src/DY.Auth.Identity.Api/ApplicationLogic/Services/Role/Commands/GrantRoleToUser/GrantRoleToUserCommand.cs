using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.GrantRoleToUser;

/// <summary>
/// Grant role to user CQRS command.
/// </summary>
public record GrantRoleToUserCommand : IRequest<ServiceResult>
{
    /// <summary>
    /// Gets role id.
    /// </summary>
    public Guid RoleId { get; init; }

    /// <summary>
    /// Gets user id.
    /// </summary>
    public Guid UserId { get; init; }
}
