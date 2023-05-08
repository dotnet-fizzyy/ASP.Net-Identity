using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Services.Common;
using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;

/// <summary>
/// Get email template by id CQRS query.
/// </summary>
public record GetEmailTemplateByIdQuery : IBaseId, IRequest<ServiceResult<EmailTemplateDto>>
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
