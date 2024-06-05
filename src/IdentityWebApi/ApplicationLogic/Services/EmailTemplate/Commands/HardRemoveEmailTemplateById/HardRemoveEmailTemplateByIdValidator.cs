using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.EmailTemplate.Commands.HardRemoveEmailTemplateById;

/// <summary>
/// <see cref="HardRemoveEmailTemplateByIdCommand"/> validator.
/// </summary>
public class HardRemoveEmailTemplateByIdValidator : AbstractValidator<HardRemoveEmailTemplateByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveEmailTemplateByIdValidator"/> class.
    /// </summary>
    public HardRemoveEmailTemplateByIdValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
