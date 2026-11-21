using AutoMapper;

using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Queries.GetRoleById;

/// <summary>
/// Get role by id CQRS handler.
/// </summary>
/// <param name="databaseContext">Instance of <see cref="DatabaseContext"/>.</param>
/// <param name="mapper">Instance of <see cref="IMapper"/>.</param>
public class GetRoleByIdQueryHandler(DatabaseContext databaseContext, IMapper mapper) : IRequestHandler<GetRoleByIdQuery, ServiceResult<GetRoleByIdResult>>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    private readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <inheritdoc />
    public async Task<ServiceResult<GetRoleByIdResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var roleEntity = await this.databaseContext.SearchByIdAsync<AppRole>(request.Id, cancellationToken);

        if (roleEntity == null)
        {
            return new ServiceResult<GetRoleByIdResult>();
        }

        var mappedRole = this.mapper.Map<GetRoleByIdResult>(roleEntity);

        return new ServiceResult<GetRoleByIdResult>(mappedRole);
    }
}
