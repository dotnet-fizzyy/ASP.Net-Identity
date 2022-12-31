using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityWebApi.ApplicationLogic.Models.Output;

namespace IdentityWebApi.ApplicationLogic.Services.User.Queries.GetUserById;

/// <summary>
/// Gets user by id query CQRS handler.
/// </summary>
public class GetsUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ServiceResult<UserResult>>
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
    public async Task<ServiceResult<UserResult>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await this.databaseContext.SearchById<AppUser>(query.Id);

        if (userEntity == null)
        {
            return new ServiceResult<UserResult>(ServiceResultType.NotFound);
        }

        var userDto = this.mapper.Map<UserResult>(userEntity);

        return new ServiceResult<UserResult>(userDto);
    }
}