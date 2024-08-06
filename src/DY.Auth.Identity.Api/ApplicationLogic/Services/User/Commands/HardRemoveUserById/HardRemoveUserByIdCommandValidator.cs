using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.HardRemoveUserById;

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
