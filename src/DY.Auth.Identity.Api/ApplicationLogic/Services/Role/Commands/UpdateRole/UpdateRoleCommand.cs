using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.UpdateRole;

/// <summary>
/// Update role CQRS command.
/// </summary>
public record UpdateRoleCommand : IBaseRequest, IRequest<ServiceResult<UpdateRoleResult>>
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
    /// Gets ro
    /// </summary>
    public string ConcurrencyStamp { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRoleCommand"/> class.
    /// </summary>
    /// <param name="name">Role name.</param>
    /// <param name="concurrencyStamp">Role entity concurrency stamp.</param>
    public UpdateRoleCommand(string name, string concurrencyStamp)
    {
        this.Name = name;
        this.ConcurrencyStamp = concurrencyStamp;
    }
}
