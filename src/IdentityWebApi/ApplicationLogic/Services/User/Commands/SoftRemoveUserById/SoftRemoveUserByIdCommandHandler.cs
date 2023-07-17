using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.SoftRemoveUserById;

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
        this.databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> Handle(SoftRemoveUserByIdCommand command, CancellationToken cancellationToken)
    {
        var user = await this.databaseContext.SearchById<AppUser>(command.Id);
        if (user == null)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        this.databaseContext.SoftRemove(user);

        await this.databaseContext.SaveChangesAsync();

        return new ServiceResult(ServiceResultType.Success);
    }
}
