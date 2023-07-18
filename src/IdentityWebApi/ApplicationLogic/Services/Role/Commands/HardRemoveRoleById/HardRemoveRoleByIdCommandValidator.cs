using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.HardRemoveRoleById;

/// <summary>
/// <see cref="HardRemoveRoleByIdCommand"/> validator.
/// </summary>
public class HardRemoveRoleByIdCommandValidator : AbstractValidator<HardRemoveRoleByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveRoleByIdCommandValidator"/> class.
    /// </summary>
    public HardRemoveRoleByIdCommandValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
