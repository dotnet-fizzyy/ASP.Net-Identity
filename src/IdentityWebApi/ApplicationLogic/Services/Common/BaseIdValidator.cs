using FluentValidation;

namespace IdentityWebApi.ApplicationLogic.Services.Common;

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