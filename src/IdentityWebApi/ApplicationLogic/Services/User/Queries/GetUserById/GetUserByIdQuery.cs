using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.ApplicationLogic.Services.Common;
using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.User.Queries.GetUserById;

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
