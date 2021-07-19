using System;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMapper _mapper;

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IMapper mapper)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _mapper = mapper;
        }
        
        public async Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id)
        {
            var emailTemplateEntity = await _emailTemplateRepository.SearchForSingleItemAsync(x => x.Id == id);

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
}