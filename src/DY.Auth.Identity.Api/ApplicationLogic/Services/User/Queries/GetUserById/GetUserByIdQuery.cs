using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Presentation.Models.DTO.User;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Queries.GetUserById;

/// <summary>
/// Get user by id CQRS query.
/// </summary>
public record GetUserByIdQuery : IBaseId, IRequest<ServiceResult<UserResult>>
{
    /// <summary>
    /// Gets user id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdQuery"/> class.
    /// </summary>
    /// <param name="id">User id.</param>
    public GetUserByIdQuery(Guid id)
    {
        this.Id = id;
    }
}
