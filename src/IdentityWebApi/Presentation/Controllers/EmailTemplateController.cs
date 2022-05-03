using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

[Authorize(Roles = UserRoleConstants.Admin)]
[Route("api/email-template")]
public class EmailTemplateController : ControllerBase
{
    private readonly IEmailTemplateService emailTemplateService;

    public EmailTemplateController(IEmailTemplateService emailTemplateService)
    {
        this.emailTemplateService = emailTemplateService;
    }

    [HttpGet("id/{id:guid}")]
    public async Task<IActionResult> GetEmailTemplate(Guid id)
    {
        var emailTemplateResult = await this.emailTemplateService.GetEmailTemplateDtoAsync(id);

        if (emailTemplateResult.IsResultNotFound)
        {
            return this.NotFound();
        }

        return this.Ok(emailTemplateResult.Data);
    }
}
