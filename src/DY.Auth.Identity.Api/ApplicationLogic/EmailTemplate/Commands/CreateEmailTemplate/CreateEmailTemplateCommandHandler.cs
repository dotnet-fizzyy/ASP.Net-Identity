using DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;

/// <summary>
/// Create email template CQRS handler.
/// </summary>
/// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
public class CreateEmailTemplateCommandHandler(DatabaseContext databaseContext)
    : IRequestHandler<CreateEmailTemplateCommand, ServiceResult<CreateEmailTemplateResult>>
{
    private readonly DatabaseContext databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

    /// <inheritdoc />
    public async Task<ServiceResult<CreateEmailTemplateResult>> Handle(
        CreateEmailTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var isEmailTemplateWithSameNameExist = await this.databaseContext.EmailTemplates
            .AnyAsync(
                emailTemplate => emailTemplate.Name.ToLower() == command.Name.ToLower(),
                cancellationToken);

        if (isEmailTemplateWithSameNameExist)
        {
            return new ServiceResult<CreateEmailTemplateResult>(
                ServiceResultType.InvalidData,
                $"Email template with name {command.Name} already exists");
        }

        var result = await this.CreateEmailTemplateAsync(command, cancellationToken);

        return new ServiceResult<CreateEmailTemplateResult>(ServiceResultType.Success, result);
    }

    private async Task<CreateEmailTemplateResult> CreateEmailTemplateAsync(
        CreateEmailTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var emailTemplateToCreate = EmailTemplateMapper.ToEmailTemplateEntity(command);

        var createdEmailTemplate = this.databaseContext.Add(emailTemplateToCreate);

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return EmailTemplateMapper.ToCreateEmailTemplateResult(createdEmailTemplate.Entity);
    }
}
