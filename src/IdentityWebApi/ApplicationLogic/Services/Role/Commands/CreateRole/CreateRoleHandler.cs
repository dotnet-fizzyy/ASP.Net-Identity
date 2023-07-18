using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.CreateRole;

/// <summary>
/// Create role CQRS handler.
/// </summary>
public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, ServiceResult<RoleResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    public CreateRoleHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />.
    public async Task<ServiceResult<RoleResult>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var isRoleWithSameNameExist = await this.SearchForExistingRoleNameAsync(command.Name, cancellationToken);

        if (isRoleWithSameNameExist)
        {
            return new ServiceResult<RoleResult>(
                ServiceResultType.InvalidData,
                $"Role with name {command.Name} already exists");
        }

        var roleToCreate = this.mapper.Map<AppRole>(command);

        var createdRole = await this.CreateRoleAsync(roleToCreate, cancellationToken);

        var roleResult = this.mapper.Map<RoleResult>(createdRole);

        return new ServiceResult<RoleResult>(ServiceResultType.Success, roleResult);
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
