using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.User.Commands.UpdateUser;

/// <summary>
/// Update user CQRS handler.
/// </summary>
/// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
public class UpdateUserCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateUserCommand, ServiceResult<UpdateUserResult>>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

    /// <inheritdoc/>
    public async Task<ServiceResult<UpdateUserResult>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var isUserExist = await this.CheckIfUserExistsAsync(command.Id, cancellationToken);

        if (!isUserExist)
        {
            return new ServiceResult<UpdateUserResult>(ServiceResultType.NotFound);
        }

        var userToUpdate = UserMapper.ToAppUser(command);

        var updatedUser = await this.UpdateUserDetailsAsync(userToUpdate, cancellationToken);

        var updatedUserResult = UserMapper.ToUpdateUserResult(updatedUser);

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
