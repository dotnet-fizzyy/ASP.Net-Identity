using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Email template controller.
/// </summary>
[Authorize(Roles = UserRoleConstants.Admin)]
[Route("api/email-template")]
public class EmailTemplateController : ControllerBase
{
    private readonly IEmailTemplateService emailTemplateService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateController"/> class.
    /// </summary>
    /// <param name="emailTemplateService"><see cref="IEmailTemplateService"/>.</param>
    public EmailTemplateController(IEmailTemplateService emailTemplateService)
    {
        this.emailTemplateService = emailTemplateService;
    }

    /// <summary>
    /// Returns email template.
    /// </summary>
    /// <response code="200">Entity has been found.</response>
    /// <response code="404">Unable to find entity.</response>
    /// <param name="id">Email template identifier.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
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
