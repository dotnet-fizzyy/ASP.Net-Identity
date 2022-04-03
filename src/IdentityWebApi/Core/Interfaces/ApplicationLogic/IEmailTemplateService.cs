using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IEmailTemplateService
{
    Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id);
}
