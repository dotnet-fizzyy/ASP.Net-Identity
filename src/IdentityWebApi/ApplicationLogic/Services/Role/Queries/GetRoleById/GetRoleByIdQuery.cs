using System;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Services.Common;
using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Queries.GetRoleById;

/// <summary>
/// Get role by id CQRS query.
/// </summary>
public record GetRoleByIdQuery : IBaseId, IRequest<ServiceResult<RoleDto>>
{
    /// <summary>
    /// Gets role id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRoleByIdQuery"/> class.
    /// </summary>
    /// <param name="id">Role id.</param>
    public GetRoleByIdQuery(Guid id)
    {
        this.Id = id;
    }
}