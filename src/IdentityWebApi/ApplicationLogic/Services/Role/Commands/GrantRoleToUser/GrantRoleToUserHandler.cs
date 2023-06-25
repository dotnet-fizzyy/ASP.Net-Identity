using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.GrantRoleToUser;

public class GrantRoleToUserHandler : IRequestHandler<GrantRoleToUserCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GrantRoleToUserHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    public GrantRoleToUserHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(GrantRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var appRole = await this.databaseContext.SearchById<AppRole>(request.RoleId);

        if (appRole is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, "No role found by provided id.");
        }

        var appUser = await this.databaseContext.SearchById<AppUser>(request.UserId);

        if (appUser is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, "No user found by provided id.");
        }

        AddNewRoleToUser(appUser, appRole);

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }

    private static void AddNewRoleToUser(AppUser appUser, AppRole appRole)
    {
        var appUserRole = new AppUserRole
        {
            Role = appRole,
            AppUser = appUser,
        };

        appUser.UserRoles.Add(appUserRole);
    }
}
