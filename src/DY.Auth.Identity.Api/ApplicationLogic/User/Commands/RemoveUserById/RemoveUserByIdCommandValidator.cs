using DY.Auth.Identity.Api.ApplicationLogic.Shared.Validators;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.User.Commands.RemoveUserById;

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
        this.Include(new BaseIdValidator());
    }
}
