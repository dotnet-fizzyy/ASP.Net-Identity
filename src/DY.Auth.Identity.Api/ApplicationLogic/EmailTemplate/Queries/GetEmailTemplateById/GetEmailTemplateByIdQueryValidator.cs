using DY.Auth.Identity.Api.ApplicationLogic.Shared.Validators;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;

/// <summary>
/// <see cref="GetEmailTemplateByIdQuery"/> validator.
/// </summary>
public class GetEmailTemplateByIdQueryValidator : AbstractValidator<GetEmailTemplateByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetEmailTemplateByIdQueryValidator"/> class.
    /// </summary>
    public GetEmailTemplateByIdQueryValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
