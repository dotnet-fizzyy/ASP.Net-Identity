using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.SoftRemoveRoleById;

/// <summary>
/// Soft remove role by id CQRS handler.
/// </summary>
public class SoftRemoveRoleByIdHandler : IRequestHandler<SoftRemoveRoleByIdCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveRoleByIdHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    public SoftRemoveRoleByIdHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(SoftRemoveRoleByIdCommand command, CancellationToken cancellationToken)
    {
        var appRole = await this.databaseContext.SearchById<AppRole>(command.Id);

        if (appRole == null)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        this.databaseContext.SoftRemove(appRole);

        await this.databaseContext.SaveChangesAsync();

        return new ServiceResult(ServiceResultType.Success);
    }
}