using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EmailTemplateEntity = DY.Auth.Identity.Api.Core.Entities.EmailTemplate;

namespace DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.HardRemoveEmailTemplateById;

/// <summary>
/// Hard remove email template by ID CQRS handler.
/// </summary>
/// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
public class HardRemoveEmailTemplateByIdHandler(DatabaseContext databaseContext) : IRequestHandler<HardRemoveEmailTemplateByIdCommand, ServiceResult>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(HardRemoveEmailTemplateByIdCommand command, CancellationToken cancellationToken)
    {
        var isEmailTemplateExisting = await this.databaseContext.ExistsByIdAsync<EmailTemplateEntity>(command.Id, cancellationToken);

        if (!isEmailTemplateExisting)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        await this.RemoveEmailTemplateAsync(command.Id, cancellationToken);

        return new ServiceResult(ServiceResultType.Success);
    }

    private async Task RemoveEmailTemplateAsync(Guid id, CancellationToken cancellationToken)
    {
        await this.databaseContext.EmailTemplates
            .Where(emailTemplate => emailTemplate.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
