using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Commands.CreateEmailTemplate;

/// <summary>
/// <see cref="CreateEmailTemplateCommand"/> validator.
/// </summary>
public class CreateEmailTemplateCommandValidator : AbstractValidator<CreateEmailTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmailTemplateCommandValidator"/> class.
    /// </summary>
    public CreateEmailTemplateCommandValidator()
    {
        this.RuleFor(command => command.Name)
            .NotNull()
            .NotEmpty();

        this.RuleFor(command => command.Layout)
            .NotNull()
            .NotEmpty()
            .MaximumLength(4000);
    }
}
