using AutoMapper;

using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

using EmailTemplateEntity = DY.Auth.Identity.Api.Core.Entities.EmailTemplate;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Commands.CreateEmailTemplate;

/// <summary>
/// Create email template CQRS handler.
/// </summary>
public class CreateEmailTemplateCommandHandler : IRequestHandler<CreateEmailTemplateCommand, ServiceResult<CreateEmailTemplateResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmailTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">The instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    public CreateEmailTemplateCommandHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

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
        var emailTemplateToCreate = this.mapper.Map<EmailTemplateEntity>(command);

        var createdEmailTemplate = this.databaseContext.Add(emailTemplateToCreate);

        await this.databaseContext.SaveChangesAsync(cancellationToken);

        return this.mapper.Map<CreateEmailTemplateResult>(createdEmailTemplate.Entity);
    }
}
