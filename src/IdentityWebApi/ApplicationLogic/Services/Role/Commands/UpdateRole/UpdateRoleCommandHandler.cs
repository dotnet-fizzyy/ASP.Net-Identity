using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.UpdateRole;

/// <summary>
/// Update role CQRS handler.
/// </summary>
public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ServiceResult<RoleResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRoleCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    public UpdateRoleCommandHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />.
    public async Task<ServiceResult<RoleResult>> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var isRoleExists = await this.databaseContext.ExistsByIdAsync<AppRole>(command.Id, cancellationToken);

        if (!isRoleExists)
        {
            return new ServiceResult<RoleResult>(ServiceResultType.NotFound);
        }

        var roleToUpdate = this.mapper.Map<AppRole>(command);

        var updatedRole = await this.UpdateRoleAsync(roleToUpdate, cancellationToken);

        var roleUpdateResult = this.mapper.Map<RoleResult>(updatedRole);

        return new ServiceResult<RoleResult>(ServiceResultType.Success, roleUpdateResult);
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
