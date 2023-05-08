using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.HardRemoveUserById;

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
