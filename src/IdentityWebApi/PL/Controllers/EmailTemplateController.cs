using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Constants;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.PL.Controllers;

[Authorize(Roles = UserRoleConstants.Admin)]
[Route("api/email-template")]
public class EmailTemplateController : ControllerBase
{
    private readonly IEmailTemplateService _emailTemplateService;

    public EmailTemplateController(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    [HttpGet("id/{id:guid}")]
    public async Task<IActionResult> GetEmailTemplate(Guid id)
    {
        var emailTemplateResult = await _emailTemplateService.GetEmailTemplateDtoAsync(id);

        if (emailTemplateResult.IsResultNotFound)
        {
            return NotFound();
        }

        return Ok(emailTemplateResult.Data);
    }
}
