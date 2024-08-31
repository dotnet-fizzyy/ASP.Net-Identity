using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Presentation.Models.DTO.EmailTemplate;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Controllers;

/// <summary>
/// Email template controller.
/// </summary>
[Authorize(Roles = UserRoleConstants.Admin)]
[Route("api/email-template")]
public class EmailTemplateController : ControllerBase
{
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateController"/> class.
    /// </summary>
    /// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    public EmailTemplateController(IMediator mediator, IMapper mapper)
        : base(mediator)
    {
        this.mapper = mapper;
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
            return this.CreateBadResponseByServiceResult(emailTemplateResult);
        }

        return this.mapper.Map<EmailTemplateDto>(emailTemplateResult.Data);
    }
}
