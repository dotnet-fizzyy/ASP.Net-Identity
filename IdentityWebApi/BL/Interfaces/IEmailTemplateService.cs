using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id);
    }
}