using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.GrantRoleToUser;

/// <summary>
/// Grant role to user CQRS handler.
/// </summary>
public class GrantRoleToUserCommandHandler : IRequestHandler<GrantRoleToUserCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GrantRoleToUserCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    public GrantRoleToUserCommandHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(GrantRoleToUserCommand request, CancellationToken cancellationToken)
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

        var assignRoleToUserResult = AddNewRoleToUser(appUser, appRole);

        if (assignRoleToUserResult.IsResultFailed)
        {
            return assignRoleToUserResult;
        }

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }

    private static ServiceResult AddNewRoleToUser(AppUser appUser, AppRole appRole)
    {
        var existingAppUserRole = appRole.UserRoles.SingleOrDefault(userRole => userRole.RoleId == appRole.Id);

        if (existingAppUserRole is not null)
        {
            return new ServiceResult(ServiceResultType.InvalidData, "User is already assigned to this role");
        }

        var appUserRole = new AppUserRole
        {
            Role = appRole,
            AppUser = appUser,
        };

        appUser.UserRoles.Add(appUserRole);

        return new ServiceResult(ServiceResultType.Success);
    }
}
