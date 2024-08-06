using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.RevokeRoleFromUser;

/// <summary>
/// Revoke role from user CQRS handler.
/// </summary>
public class RevokeRoleFromUserCommandHandler : IRequestHandler<RevokeRoleFromUserCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RevokeRoleFromUserCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    public RevokeRoleFromUserCommandHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(RevokeRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        var appRole = await this.databaseContext.SearchByIdAsync<AppRole>(request.RoleId, cancellationToken);

        if (appRole is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, "No role found by provided id.");
        }

        var appUser =
              await this.databaseContext.SearchByIdAsync<AppUser>(
                request.UserId,
                includeTracking: true,
                cancellationToken,
                include => include.UserRoles);

        if (appUser is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, "No user found by provided id.");
        }

        var revokeRoleFromUserResult = RevokeRoleFromUser(appUser, appRole);

        if (revokeRoleFromUserResult.IsResultFailed)
        {
            return revokeRoleFromUserResult;
        }

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }

    private static ServiceResult RevokeRoleFromUser(AppUser appUser, AppRole appRole)
    {
        var appUserRole = appRole.UserRoles.SingleOrDefault(userRole => userRole.RoleId == appRole.Id);

        if (appUserRole is null)
        {
            return new ServiceResult(ServiceResultType.InvalidData, "User is not assigned to this role");
        }

        appUser.UserRoles.Remove(appUserRole);

        return new ServiceResult(ServiceResultType.Success);
    }
}
