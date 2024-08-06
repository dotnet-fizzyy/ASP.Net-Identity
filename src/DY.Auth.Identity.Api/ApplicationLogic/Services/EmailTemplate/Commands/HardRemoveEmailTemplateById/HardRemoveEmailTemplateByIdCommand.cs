using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Commands.HardRemoveEmailTemplateById;

/// <summary>
/// Hard remove email template by ID CQRS command.
/// </summary>
public record HardRemoveEmailTemplateByIdCommand : IBaseId, IRequest<ServiceResult>
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveEmailTemplateByIdCommand"/> class.
    /// </summary>
    /// <param name="id">Given email template Id to remove.</param>
    public HardRemoveEmailTemplateByIdCommand(Guid id)
    {
        this.Id = id;
    }
}
