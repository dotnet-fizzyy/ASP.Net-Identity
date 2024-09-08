using AutoMapper;

using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.UpdateUser;

/// <summary>
/// Update user CQRS handler.
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResult<UpdateUserResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public UpdateUserCommandHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UpdateUserResult>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var isUserExist = await this.CheckIfUserExistsAsync(command.Id, cancellationToken);

        if (!isUserExist)
        {
            return new ServiceResult<UpdateUserResult>(ServiceResultType.NotFound);
        }

        var userToUpdate = this.mapper.Map<AppUser>(command);

        var updatedUser = await this.UpdateUserDetailsAsync(userToUpdate, cancellationToken);

        var updatedUserResult = this.mapper.Map<UpdateUserResult>(updatedUser);

        return new ServiceResult<UpdateUserResult>(ServiceResultType.Success, updatedUserResult);
    }

    private Task<bool> CheckIfUserExistsAsync(Guid id, CancellationToken cancellationToken) =>
        this.databaseContext.ExistsByIdAsync<AppUser>(id, cancellationToken);

    private async Task<AppUser> UpdateUserDetailsAsync(AppUser user, CancellationToken cancellationToken)
    {
        var userEntry = this.databaseContext.Entry(user);

        userEntry.Property(prop => prop.UserName).IsModified = true;
        userEntry.Property(prop => prop.PhoneNumber).IsModified = true;
        userEntry.Property(prop => prop.Email).IsModified = true;

        await this.databaseContext.SaveChangesAsync(cancellationToken);
        await userEntry.ReloadAsync(cancellationToken);

        userEntry.State = EntityState.Detached;

        return user;
    }
}
