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
public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, ServiceResult<RoleResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRoleByIdHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">Instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="mapper">Instance of <see cref="IMapper"/>.</param>
    public GetRoleByIdHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<ServiceResult<RoleResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var roleEntity = await this.databaseContext.SearchById<AppRole>(request.Id);

        if (roleEntity == null)
        {
            return new ServiceResult<RoleResult>();
        }

        var mappedRole = this.mapper.Map<RoleResult>(roleEntity);

        return new ServiceResult<RoleResult>(mappedRole);
    }
}
