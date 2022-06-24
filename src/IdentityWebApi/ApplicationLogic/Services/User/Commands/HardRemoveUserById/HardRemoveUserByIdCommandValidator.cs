using FluentValidation;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.HardRemoveUserById;

// todo: Create base validator

/// <summary>
/// <see cref="HardRemoveUserByIdCommand"/> validator.
/// </summary>
public class HardRemoveUserByIdCommandValidator : AbstractValidator<HardRemoveUserByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveUserByIdCommandValidator"/> class.
    /// </summary>
    public HardRemoveUserByIdCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty();
    }
}