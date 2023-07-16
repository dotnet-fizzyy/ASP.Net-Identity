using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.RevokeRoleFromUser;

/// <summary>
/// Revoke role from user CQRS command.
/// </summary>
public record RevokeRoleFromUserCommand : IRequest<ServiceResult>
{
    /// <summary>
    /// Gets or sets role id.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RevokeRoleFromUserCommand"/> class.
    /// </summary>
    /// <param name="userId">User id to be assigned with role.</param>
    /// <param name="roleId">Role id to assign.</param>
    public RevokeRoleFromUserCommand(Guid userId, Guid roleId)
    {
        this.UserId = userId;
        this.RoleId = roleId;
    }
}
