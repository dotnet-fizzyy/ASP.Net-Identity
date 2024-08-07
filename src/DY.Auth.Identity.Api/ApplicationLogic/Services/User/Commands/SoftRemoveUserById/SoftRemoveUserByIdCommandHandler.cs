using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.SoftRemoveUserById;

/// <summary>
/// Soft remove user by id CQRS command handler.
/// </summary>
public class SoftRemoveUserByIdCommandHandler : IRequestHandler<SoftRemoveUserByIdCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveUserByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    public SoftRemoveUserByIdCommandHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> Handle(SoftRemoveUserByIdCommand command, CancellationToken cancellationToken)
    {
        var user = await this.databaseContext.SearchByIdAsync<AppUser>(command.Id, cancellationToken);
        if (user == null)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        this.databaseContext.SoftRemove(user);

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }
}
