using AutoMapper;

using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.UpdateRole;

/// <summary>
/// Update role CQRS handler.
/// </summary>
public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ServiceResult<UpdateRoleResult>>
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
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />.
    public async Task<ServiceResult<UpdateRoleResult>> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var isRoleExists = await this.databaseContext.ExistsByIdAsync<AppRole>(command.Id, cancellationToken);

        if (!isRoleExists)
        {
            return new ServiceResult<UpdateRoleResult>(ServiceResultType.NotFound);
        }

        var roleToUpdate = this.mapper.Map<AppRole>(command);

        var updatedRole = await this.UpdateRoleAsync(roleToUpdate, cancellationToken);

        var roleUpdateResult = this.mapper.Map<UpdateRoleResult>(updatedRole);

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
