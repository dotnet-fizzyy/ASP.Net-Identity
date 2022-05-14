using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

/// <summary>
/// EmailTemplate service.
/// </summary>
public interface IEmailTemplateService
{
    /// <summary>
    /// Gets EmailTemplate by id.
    /// </summary>
    /// <param name="id">EmailTemplate id.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with email template.</returns>
    Task<ServiceResult<EmailTemplateDto>> GetEmailTemplateDtoAsync(Guid id);
}
