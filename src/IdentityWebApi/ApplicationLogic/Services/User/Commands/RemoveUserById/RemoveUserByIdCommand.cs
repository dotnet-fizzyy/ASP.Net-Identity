using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.RemoveUserById;

/// <summary>
/// Remove user by id CQRS command.
/// </summary>
public class RemoveUserByIdCommand : IRequest<ServiceResult>
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