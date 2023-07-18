using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Queries.GetRoleById;

/// <summary>
/// Get role by id CQRS handler.
/// </summary>
public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ServiceResult<RoleResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRoleByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">Instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="mapper">Instance of <see cref="IMapper"/>.</param>
    public GetRoleByIdQueryHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<ServiceResult<RoleResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var roleEntity = await this.databaseContext.SearchByIdAsync<AppRole>(request.Id, cancellationToken);

        if (roleEntity == null)
        {
            return new ServiceResult<RoleResult>();
        }

        var mappedRole = this.mapper.Map<RoleResult>(roleEntity);

        return new ServiceResult<RoleResult>(mappedRole);
    }
}
