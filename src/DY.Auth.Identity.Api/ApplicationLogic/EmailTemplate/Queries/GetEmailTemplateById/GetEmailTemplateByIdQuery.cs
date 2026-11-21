using DY.Auth.Identity.Api.ApplicationLogic.Shared.Requests;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;

/// <summary>
/// Get email template by id CQRS query.
/// </summary>
public record GetEmailTemplateByIdQuery : IBaseId, IRequest<ServiceResult<GetEmailTemplateByIdResult>>
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
