using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.User.Queries.GetUserById;

/// <summary>
/// Get user by id CQRS query.
/// </summary>
public record GetUserByIdQuery : IRequest<ServiceResult<UserResultDto>>
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