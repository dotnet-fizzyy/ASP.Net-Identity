using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.UpdateRole;

/// <summary>
/// Update role CQRS command.
/// </summary>
public record UpdateRoleCommand : IRequest<ServiceResult<RoleResult>>
{
    /// <summary>
    /// Gets or sets role id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets role name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRoleCommand"/> class.
    /// </summary>
    /// <param name="name">Role name.</param>
    public UpdateRoleCommand(string name)
    {
        this.Name = name;
    }
}
