using FluentValidation;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.RemoveUserById;

// todo: Create base validator

/// <summary>
/// <see cref="RemoveUserByIdCommand"/> validator.
/// </summary>
public class RemoveUserByIdCommandValidator : AbstractValidator<RemoveUserByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveUserByIdCommandValidator"/> class.
    /// </summary>
    public RemoveUserByIdCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty();
    }
}