using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;

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
