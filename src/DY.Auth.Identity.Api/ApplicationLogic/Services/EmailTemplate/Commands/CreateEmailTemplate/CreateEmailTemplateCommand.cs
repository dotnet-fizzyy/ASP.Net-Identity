using DY.Auth.Identity.Api.Core.Results;

using MediatR;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Commands.CreateEmailTemplate;

/// <summary>
/// Create email template CQRS command.
/// </summary>
public record CreateEmailTemplateCommand : IRequest<ServiceResult<CreateEmailTemplateResult>>
{
    /// <summary>
    /// Gets email template name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets email template layout.
    /// </summary>
    public string Layout { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmailTemplateCommand"/> class.
    /// </summary>
    /// <param name="name">Email template name.</param>
    /// <param name="layout">Email template layout.</param>
    public CreateEmailTemplateCommand(string name, string layout)
    {
        this.Name = name;
        this.Layout = layout;
    }
}
