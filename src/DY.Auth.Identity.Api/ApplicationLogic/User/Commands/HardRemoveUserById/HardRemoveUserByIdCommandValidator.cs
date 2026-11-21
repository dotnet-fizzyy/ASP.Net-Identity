using DY.Auth.Identity.Api.ApplicationLogic.Shared.Validators;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.User.Commands.HardRemoveUserById;

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
        this.Include(new BaseIdValidator());
    }
}
