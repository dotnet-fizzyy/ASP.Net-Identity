using IdentityWebApi.ApplicationLogic.EmailTemplate.GetEmailTemplateById;
using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Constants;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateController"/> class.
    /// </summary>
    /// <param name="mediator"><see cref="IMediator"/>.</param>
    public EmailTemplateController(IMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Returns email template.
    /// </summary>
    /// <response code="200">Entity has been found.</response>
    /// <response code="404">Unable to find entity.</response>
    /// <param name="id">Email template identifier.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(EmailTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmailTemplateDto>> GetEmailTemplate(Guid id)
    {
        var query = new GetEmailTemplateByIdQuery(id);
        var emailTemplateResult = await this.Mediator.Send(query);

        if (emailTemplateResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(emailTemplateResult);
        }

        return emailTemplateResult.Data;
    }
}
