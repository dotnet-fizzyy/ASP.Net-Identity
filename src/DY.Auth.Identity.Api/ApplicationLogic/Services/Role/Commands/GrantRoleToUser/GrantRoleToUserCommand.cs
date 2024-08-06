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
    /// Gets or sets role id.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GrantRoleToUserCommand"/> class.
    /// </summary>
    /// <param name="userId">User id to be assigned with role.</param>
    /// <param name="roleId">Role id to assign.</param>
    public GrantRoleToUserCommand(Guid userId, Guid roleId)
    {
        this.UserId = userId;
        this.RoleId = roleId;
    }
}
