using DY.Auth.Identity.Api.ApplicationLogic.Shared.Requests;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Shared.Validators;

/// <summary>
/// <see cref="IBaseId"/> validator.
/// </summary>
public class BaseIdValidator : AbstractValidator<IBaseId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseIdValidator"/> class.
    /// </summary>
    public BaseIdValidator()
    {
        this.RuleFor(prop => prop.Id)
            .NotEmpty();
    }
}
