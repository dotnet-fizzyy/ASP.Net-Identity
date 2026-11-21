using DY.Auth.Identity.Api.ApplicationLogic.Shared.Requests;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.User.Commands.RemoveUserById;

/// <summary>
/// Remove user by id CQRS command.
/// </summary>
public record RemoveUserByIdCommand : IBaseId, IRequest<ServiceResult>
{
    /// <summary>
    /// Gets user id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveUserByIdCommand"/> class.
    /// </summary>
    /// <param name="id">User id.</param>
    public RemoveUserByIdCommand(Guid id)
    {
        this.Id = id;
    }
}
