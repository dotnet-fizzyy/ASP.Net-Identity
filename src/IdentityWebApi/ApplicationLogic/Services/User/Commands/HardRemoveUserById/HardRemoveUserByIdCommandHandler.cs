using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.HardRemoveUserById;

/// <summary>
/// Remove user by id query CQRS handler.
/// </summary>
public class HardRemoveUserByIdCommandHandler : IRequestHandler<HardRemoveUserByIdCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveUserByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    public HardRemoveUserByIdCommandHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> Handle(HardRemoveUserByIdCommand request, CancellationToken cancellationToken)
    {
        var appUser = await this.GetAppUserAsync(request.Id);

        if (appUser == null)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        await this.RemoveUserAsync(appUser);

        return new ServiceResult(ServiceResultType.Success);
    }

    private async Task<AppUser> GetAppUserAsync(Guid id) =>
        await this.databaseContext.SearchById<AppUser>(
            id,
            includeTracking: true,
            includedEntity => includedEntity.UserRoles);

    private async Task RemoveUserAsync(AppUser appUser)
    {
        this.databaseContext.UserRoles.RemoveRange(appUser.UserRoles);
        this.databaseContext.Users.Remove(appUser);

        await this.databaseContext.SaveChangesAsync();
    }
}