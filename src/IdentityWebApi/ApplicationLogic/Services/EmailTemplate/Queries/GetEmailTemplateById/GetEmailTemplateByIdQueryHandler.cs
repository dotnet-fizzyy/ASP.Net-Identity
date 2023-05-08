using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;

/// <summary>
/// Gets email template by id query CQRS handler.
/// </summary>
public class
    GetEmailTemplateByIdQueryHandler : IRequestHandler<GetEmailTemplateByIdQuery, ServiceResult<EmailTemplateDto>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEmailTemplateByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public GetEmailTemplateByIdQueryHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<EmailTemplateDto>> Handle(
        GetEmailTemplateByIdQuery query, CancellationToken cancellationToken)
    {
        var emailTemplateEntity = await this.databaseContext.SearchById<Core.Entities.EmailTemplate>(query.Id);

        if (emailTemplateEntity == null)
        {
            return new ServiceResult<EmailTemplateDto>(ServiceResultType.NotFound);
        }

        var emailTemplateDto = this.mapper.Map<EmailTemplateDto>(emailTemplateEntity);

        return new ServiceResult<EmailTemplateDto>(emailTemplateDto);
    }
}
