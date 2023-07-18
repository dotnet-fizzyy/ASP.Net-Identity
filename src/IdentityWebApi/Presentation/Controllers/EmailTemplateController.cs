using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;
using IdentityWebApi.Core.Constants;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
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
    /// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
    public EmailTemplateController(IMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Returns email template.
    /// </summary>
    /// <param name="id">Email template identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Email template has been found.</response>
    /// <response code="404">Unable to find email template.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(EmailTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmailTemplateDto>> GetEmailTemplate(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetEmailTemplateByIdQuery(id);
        var emailTemplateResult = await this.Mediator.Send(query, cancellationToken);

        if (emailTemplateResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(emailTemplateResult);
        }

        return emailTemplateResult.Data;
    }
}
