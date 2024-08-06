using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.SoftRemoveRoleById;

/// <summary>
/// Soft remove role by id CQRS handler.
/// </summary>
public class SoftRemoveRoleByIdCommandHandler : IRequestHandler<SoftRemoveRoleByIdCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveRoleByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    public SoftRemoveRoleByIdCommandHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(SoftRemoveRoleByIdCommand command, CancellationToken cancellationToken)
    {
        var appRole = await this.databaseContext.SearchByIdAsync<AppRole>(command.Id, cancellationToken);

        if (appRole == null)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        this.databaseContext.SoftRemove(appRole);

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }
}
