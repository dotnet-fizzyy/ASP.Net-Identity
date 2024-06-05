using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.HardRemoveRoleById;

/// <summary>
/// Hard remove role by id CQRS handler.
/// </summary>
public class HardRemoveRoleByIdCommandHandler : IRequestHandler<HardRemoveRoleByIdCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveRoleByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    public HardRemoveRoleByIdCommandHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(HardRemoveRoleByIdCommand command, CancellationToken cancellationToken)
    {
        var isRoleExisting = await this.databaseContext.ExistsByIdAsync<AppRole>(command.Id, cancellationToken);

        if (!isRoleExisting)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        await this.RemoveRoleAsync(command.Id, cancellationToken);

        return new ServiceResult(ServiceResultType.NoContent);
    }

    private async Task RemoveRoleAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var transaction = await this.databaseContext.Database.BeginTransactionAsync(cancellationToken);

        await this.databaseContext.UserRoles.Where(userRole => userRole.RoleId == id).ExecuteDeleteAsync(cancellationToken);
        await this.databaseContext.Roles.Where(role => role.Id == id).ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}
