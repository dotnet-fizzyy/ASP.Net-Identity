using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;
using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;
using DY.Auth.Identity.Api.Presentation.Models.DTO.EmailTemplate;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Controllers;

/// <summary>
/// Email template controller.
/// </summary>
/// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
/// <param name="controllersLinkService">The instance of <see cref="IControllersLinkService"/>.</param>
/// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
[Authorize(Roles = UserRoleConstants.Admin)]
[Route("api/email-template")]
public class EmailTemplateController(IMediator mediator, IControllersLinkService controllersLinkService, IMapper mapper) : ControllerBase(mediator)
{
    private readonly IControllersLinkService controllersLinkService = controllersLinkService ?? throw new ArgumentNullException(nameof(controllersLinkService));
    private readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

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
            return this.CreateErrorResponse(emailTemplateResult);
        }

        return this.mapper.Map<EmailTemplateDto>(emailTemplateResult.Data);
    }

    /// <summary>
    /// Creates an email template.
    /// </summary>
    /// <param name="requestBody"><see cref="CreateEmailTemplateDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">Email template has been created.</response>
    /// <response code="400">Email template with the same name already exists.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EmailTemplateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmailTemplateDto>> CreateEmailTemplate(
        [FromBody, BindRequired] CreateEmailTemplateDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<CreateEmailTemplateCommand>(requestBody);
        var creationResult = await this.Mediator.Send(command, cancellationToken);

        if (creationResult.IsResultFailed)
        {
            return this.CreateErrorResponse(creationResult);
        }

        var createdEmailTemplateDto = this.mapper.Map<EmailTemplateDto>(creationResult.Data);

        var getEmailTemplateLink = this.controllersLinkService.GenerateEmailTemplateLink(createdEmailTemplateDto.EmailTemplateId);

        return this.Created(getEmailTemplateLink, createdEmailTemplateDto);
    }
}
