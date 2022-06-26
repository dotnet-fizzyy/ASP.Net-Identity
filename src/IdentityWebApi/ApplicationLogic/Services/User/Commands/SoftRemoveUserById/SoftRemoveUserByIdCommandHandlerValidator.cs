using FluentValidation;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.SoftRemoveUserById;

// todo: Create base validator

/// <summary>
/// <see cref="SoftRemoveUserByIdCommand"/> validator.
/// </summary>
public class SoftRemoveUserByIdCommandHandlerValidator : AbstractValidator<SoftRemoveUserByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveUserByIdCommandHandlerValidator"/> class.
    /// </summary>
    public SoftRemoveUserByIdCommandHandlerValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty();
    }
}