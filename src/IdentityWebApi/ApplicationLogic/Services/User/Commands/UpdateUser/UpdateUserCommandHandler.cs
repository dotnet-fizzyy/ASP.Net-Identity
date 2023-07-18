using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.UpdateUser;

/// <summary>
/// Update user CQRS handler.
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResult<UserResult>>
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
        this.databaseContext = databaseContext;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserResult>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var isUserExist = await this.CheckIfUserExistsAsync(command.Id);

        if (!isUserExist)
        {
            return new ServiceResult<UserResult>(ServiceResultType.NotFound);
        }

        var userToUpdate = this.mapper.Map<AppUser>(command);

        var updatedUser = await this.UpdateUserDetailsAsync(userToUpdate);

        var updatedUserResult = this.mapper.Map<UserResult>(updatedUser);

        return new ServiceResult<UserResult>(ServiceResultType.Success, updatedUserResult);
    }

    private Task<bool> CheckIfUserExistsAsync(Guid id) =>
        this.databaseContext.ExistsByIdAsync<AppUser>(id);

    private async Task<AppUser> UpdateUserDetailsAsync(AppUser user)
    {
        var userEntry = this.databaseContext.Entry(user);

        userEntry.Property(prop => prop.UserName).IsModified = true;
        userEntry.Property(prop => prop.PhoneNumber).IsModified = true;
        userEntry.Property(prop => prop.Email).IsModified = true;

        await this.databaseContext.SaveChangesAsync();
        await userEntry.ReloadAsync();

        userEntry.State = EntityState.Detached;

        return user;
    }
}
