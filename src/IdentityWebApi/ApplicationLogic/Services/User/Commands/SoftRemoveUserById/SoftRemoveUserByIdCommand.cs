using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.SoftRemoveUserById;

/// <summary>
/// Soft remove user by id CQRS command.
/// </summary>
public record SoftRemoveUserByIdCommand : IRequest<ServiceResult>
{
    /// <summary>
    /// Gets user id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveUserByIdCommand"/> class.
    /// </summary>
    /// <param name="id">User id.</param>
    public SoftRemoveUserByIdCommand(Guid id)
    {
        this.Id = id;
    }
};