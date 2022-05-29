using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.EmailTemplate.GetEmailTemplateById;

public record GetEmailTemplateByIdQuery : IRequest<ServiceResult<EmailTemplateDto>>
{
    /// <summary>
    /// Gets email template id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEmailTemplateByIdQuery"/> class.
    /// </summary>
    /// <param name="id">User id.</param>
    public GetEmailTemplateByIdQuery(Guid id)
    {
        this.Id = id;
    }
}