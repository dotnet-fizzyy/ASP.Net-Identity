using AutoMapper;

using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Presentation.Models.DTO;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmailTemplateService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id)
    {
        var emailTemplateEntity = await _unitOfWork.EmailTemplateRepository.SearchForSingleItemAsync(x => x.Id == id);

        if (emailTemplateEntity is null)
        {
            return new ServiceResult<EmailTemplateDto>(ServiceResultType.NotFound);
        }

        return new ServiceResult<EmailTemplateDto>(
            ServiceResultType.Success,
            _mapper.Map<EmailTemplateDto>(emailTemplateEntity)
        );
    }
}
