using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.BL.Interfaces;

public interface IEmailTemplateService
{
    Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id);
}
