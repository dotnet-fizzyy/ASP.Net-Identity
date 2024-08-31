using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Queries.GetRoleById;

/// <summary>
/// Get role by id CQRS query.
/// </summary>
public record GetRoleByIdQuery : IBaseId, IRequest<ServiceResult<GetRoleByIdResult>>
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
