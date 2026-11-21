using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.UpdateRole;

/// <summary>
/// Update role CQRS handler.
/// </summary>
/// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
public class UpdateRoleCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateRoleCommand, ServiceResult<UpdateRoleResult>>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

    /// <inheritdoc />.
    public async Task<ServiceResult<UpdateRoleResult>> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var isRoleExists = await this.databaseContext.ExistsByIdAsync<AppRole>(command.Id, cancellationToken);

        if (!isRoleExists)
        {
            return new ServiceResult<UpdateRoleResult>(ServiceResultType.NotFound);
        }

        var roleToUpdate = RoleMapper.ToAppRole(command);

        var updatedRole = await this.UpdateRoleAsync(roleToUpdate, cancellationToken);

        var roleUpdateResult = RoleMapper.ToUpdateRoleResult(updatedRole);

        return new ServiceResult<UpdateRoleResult>(ServiceResultType.Success, roleUpdateResult);
    }

    private async Task<AppRole> UpdateRoleAsync(AppRole appRole, CancellationToken cancellationToken)
    {
        var appRoleEntry = this.databaseContext.Entry(appRole);

        appRoleEntry.Property(role => role.Name).IsModified = true;
        appRoleEntry.Property(role => role.ConcurrencyStamp).IsModified = true;

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return appRole;
    }
}
