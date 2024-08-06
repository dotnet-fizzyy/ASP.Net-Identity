using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;

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
