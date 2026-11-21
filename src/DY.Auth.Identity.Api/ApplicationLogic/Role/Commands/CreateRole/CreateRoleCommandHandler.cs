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

namespace DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.CreateRole;

/// <summary>
/// Create role CQRS handler.
/// </summary>
/// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
public class CreateRoleCommandHandler(DatabaseContext databaseContext) : IRequestHandler<CreateRoleCommand, ServiceResult<CreateRoleResult>>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

    /// <inheritdoc />.
    public async Task<ServiceResult<CreateRoleResult>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var isRoleWithSameNameExist = await this.SearchForExistingRoleNameAsync(command.Name, cancellationToken);

        if (isRoleWithSameNameExist)
        {
            return new ServiceResult<CreateRoleResult>(
                ServiceResultType.InvalidData,
                $"Role with name {command.Name} already exists");
        }

        var roleToCreate = RoleMapper.ToAppRole(command);

        var createdRole = await this.CreateRoleAsync(roleToCreate, cancellationToken);

        var roleResult = RoleMapper.ToCreateRoleResult(createdRole);

        return new ServiceResult<CreateRoleResult>(ServiceResultType.Success, roleResult);
    }

    private Task<bool> SearchForExistingRoleNameAsync(string name, CancellationToken cancellationToken) =>
        this.databaseContext.Roles.AnyAsync(
            role => role.Name.ToLower() == name.ToLower(), cancellationToken);

    private async Task<AppRole> CreateRoleAsync(AppRole appRole, CancellationToken cancellationToken)
    {
        var createdRole = await this.databaseContext.AddAsync(appRole, cancellationToken);

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return createdRole.Entity;
    }
}
