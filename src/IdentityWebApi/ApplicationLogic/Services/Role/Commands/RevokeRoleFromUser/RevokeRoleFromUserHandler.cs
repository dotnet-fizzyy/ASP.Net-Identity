using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.RevokeRoleFromUser;

/// <summary>
/// Revoke role from user CQRS handler.
/// </summary>
public class RevokeRoleFromUserHandler : IRequestHandler<RevokeRoleFromUserCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RevokeRoleFromUserHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    public RevokeRoleFromUserHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(RevokeRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        var appRole = await this.databaseContext.SearchById<AppRole>(request.RoleId);

        if (appRole is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, "No role found by provided id.");
        }

        var appUser =
              await this.databaseContext.SearchById<AppUser>(
                request.UserId,
                includeTracking: true,
                include => include.UserRoles);

        if (appUser is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, "No user found by provided id.");
        }

        var revokeRoleFromUserResult = RevokeRoleFromUser(appUser, appRole);

        if (revokeRoleFromUserResult.IsResultFailed)
        {
            return revokeRoleFromUserResult.GenerateErrorResult();
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
