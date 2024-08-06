using DY.Auth.Identity.Api.Core.Constants;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.ConfirmEmail;

/// <summary>
/// <see cref="ConfirmEmailCommand"/> validator.
/// </summary>
public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmEmailCommandValidator"/> class.
    /// </summary>
    public ConfirmEmailCommandValidator()
    {
        this.RuleFor(prop => prop.Email)
            .NotEmpty()
            .WithMessage(ValidationConstants.NullOrEmptyValue);

        this.RuleFor(prop => prop.Token)
            .NotEmpty()
            .WithMessage(ValidationConstants.NullOrEmptyValue);
    }
}
