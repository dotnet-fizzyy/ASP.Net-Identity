using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public EmailTemplateService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

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
