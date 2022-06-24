using IdentityWebApi.Core.Results;

using System;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.HardRemoveUserById;

/// <summary>
/// Remove user by id from DB CQRS command.
/// </summary>
public record HardRemoveUserByIdCommand : IRequest<ServiceResult>
{
    /// <summary>
    /// Gets user id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveUserByIdCommand"/> class.
    /// </summary>
    /// <param name="id">User id.</param>
    public HardRemoveUserByIdCommand(Guid id)
    {
        this.Id = id;
    }
}