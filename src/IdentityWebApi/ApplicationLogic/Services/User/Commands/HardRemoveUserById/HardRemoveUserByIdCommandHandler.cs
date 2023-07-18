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
/// Hard remove user by id CQRS command handler.
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
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> Handle(HardRemoveUserByIdCommand request, CancellationToken cancellationToken)
    {
        var appUser = await this.GetAppUserAsync(request.Id, cancellationToken);

        if (appUser == null)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        await this.RemoveUserAsync(appUser, cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }

    private async Task<AppUser> GetAppUserAsync(Guid id, CancellationToken cancellationToken) =>
        await this.databaseContext.SearchByIdAsync<AppUser>(
            id,
            includeTracking: true,
            cancellationToken,
            includedEntity => includedEntity.UserRoles);

    private async Task RemoveUserAsync(AppUser appUser, CancellationToken cancellationToken)
    {
        this.databaseContext.UserRoles.RemoveRange(appUser.UserRoles);
        this.databaseContext.Users.Remove(appUser);

        await this.databaseContext.SaveChangesAsync(cancellationToken);
    }
}
