using DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

using EmailTemplateEntity = DY.Auth.Identity.Api.Core.Entities.EmailTemplate;

namespace DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;

/// <summary>
/// Gets email template by id query CQRS handler.
/// </summary>
/// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
public class GetEmailTemplateByIdQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetEmailTemplateByIdQuery, ServiceResult<GetEmailTemplateByIdResult>>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

    /// <inheritdoc/>
    public async Task<ServiceResult<GetEmailTemplateByIdResult>> Handle(GetEmailTemplateByIdQuery query, CancellationToken cancellationToken)
    {
        var emailTemplateEntity = await this.databaseContext.SearchByIdAsync<EmailTemplateEntity>(query.Id, cancellationToken);

        if (emailTemplateEntity == null)
        {
            return new ServiceResult<GetEmailTemplateByIdResult>(ServiceResultType.NotFound);
        }

        var emailTemplateDto = EmailTemplateMapper.ToGetEmailTemplateByIdResult(emailTemplateEntity);

        return new ServiceResult<GetEmailTemplateByIdResult>(emailTemplateDto);
    }
}
