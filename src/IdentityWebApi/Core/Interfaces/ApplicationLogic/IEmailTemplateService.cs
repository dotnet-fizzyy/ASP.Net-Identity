using IdentityWebApi.Core.Results;
using System;
using System.Threading.Tasks;
using IdentityWebApi.Presentation.Models.DTO;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IEmailTemplateService
{
    Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id);
}
