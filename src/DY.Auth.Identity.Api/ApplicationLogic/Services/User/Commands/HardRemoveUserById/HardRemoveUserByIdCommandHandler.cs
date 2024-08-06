using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.HardRemoveUserById;

/// <summary>
/// Hard remove user by id CQRS command handler.
/// </summary>
public class HardRemoveUserByIdCommandHandler : IRequestHandler<HardRemoveUserByIdCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveUserByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    public HardRemoveUserByIdCommandHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> Handle(HardRemoveUserByIdCommand command, CancellationToken cancellationToken)
    {
        var isUserExisting = await this.databaseContext.ExistsByIdAsync<AppUser>(command.Id, cancellationToken);

        if (!isUserExisting)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        await this.RemoveUserAsync(command.Id, cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }

    private async Task RemoveUserAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var transaction = await this.databaseContext.Database.BeginTransactionAsync(cancellationToken);

        await this.databaseContext.UserRoles.Where(userRole => userRole.UserId == id).ExecuteDeleteAsync(cancellationToken);
        await this.databaseContext.Users.Where(user => user.Id == id).ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}
