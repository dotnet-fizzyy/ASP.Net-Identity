using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.HardRemoveRoleById;

/// <summary>
/// <see cref="HardRemoveRoleByIdCommand"/> validator.
/// </summary>
public class HardRemoveRoleByIdValidator : AbstractValidator<HardRemoveRoleByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HardRemoveRoleByIdValidator"/> class.
    /// </summary>
    public HardRemoveRoleByIdValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
