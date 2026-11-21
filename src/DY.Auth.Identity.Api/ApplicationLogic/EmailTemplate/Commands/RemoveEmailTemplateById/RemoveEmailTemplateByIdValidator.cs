using DY.Auth.Identity.Api.ApplicationLogic.Shared.Validators;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.RemoveEmailTemplateById;

/// <summary>
/// <see cref="RemoveEmailTemplateByIdCommand"/> validator.
/// </summary>
public class RemoveEmailTemplateByIdValidator : AbstractValidator<RemoveEmailTemplateByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveEmailTemplateByIdValidator"/> class.
    /// </summary>
    public RemoveEmailTemplateByIdValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
