using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.RevokeRoleFromUser;

/// <summary>
/// Revoke role from user CQRS command.
/// </summary>
public record RevokeRoleFromUserCommand : IRequest<ServiceResult>
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
