using AutoMapper;

using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Queries.GetUserById;

/// <summary>
/// Gets user by id query CQRS handler.
/// </summary>
public class GetsUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ServiceResult<GetUserByIdResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetsUserByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public GetsUserByIdQueryHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<GetUserByIdResult>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await this.databaseContext.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .AsSplitQuery()
            .SingleOrDefaultAsync(user => user.Id == query.Id, cancellationToken);

        if (userEntity == null)
        {
            return new ServiceResult<GetUserByIdResult>(ServiceResultType.NotFound);
        }

        var userDto = this.mapper.Map<GetUserByIdResult>(userEntity);

        return new ServiceResult<GetUserByIdResult>(userDto);
    }
}
