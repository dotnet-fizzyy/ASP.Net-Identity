using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services;

/// <inheritdoc cref="IEmailTemplateService" />
public class EmailTemplateService : IEmailTemplateService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateService"/> class.
    /// </summary>
    /// <param name="unitOfWork"><see cref="IUnitOfWork"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public EmailTemplateService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id)
    {
        var emailTemplateEntity = await this.unitOfWork.EmailTemplateRepository.SearchForSingleItemAsync(x => x.Id == id);

        if (emailTemplateEntity is null)
        {
            return new ServiceResult<EmailTemplateDto>(ServiceResultType.NotFound);
        }

        return new ServiceResult<EmailTemplateDto>(
            ServiceResultType.Success,
            this.mapper.Map<EmailTemplateDto>(emailTemplateEntity)
        );
    }
}
