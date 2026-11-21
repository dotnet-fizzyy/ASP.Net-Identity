using DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;
using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Role.Queries.GetRoleById;

/// <summary>
/// Get role by id CQRS handler.
/// </summary>
/// <param name="databaseContext">Instance of <see cref="DatabaseContext"/>.</param>
public class GetRoleByIdQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetRoleByIdQuery, ServiceResult<GetRoleByIdResult>>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

    /// <inheritdoc />
    public async Task<ServiceResult<GetRoleByIdResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var roleEntity = await this.databaseContext.SearchByIdAsync<AppRole>(request.Id, cancellationToken);

        if (roleEntity == null)
        {
            return new ServiceResult<GetRoleByIdResult>();
        }

        var mappedRole = RoleMapper.ToGetRoleByIdResult(roleEntity);

        return new ServiceResult<GetRoleByIdResult>(mappedRole);
    }
}
