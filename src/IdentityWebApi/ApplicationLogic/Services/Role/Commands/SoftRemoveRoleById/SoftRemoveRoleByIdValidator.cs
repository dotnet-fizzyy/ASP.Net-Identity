using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.SoftRemoveRoleById;

/// <summary>
/// <see cref="SoftRemoveRoleByIdCommand"/> validator.
/// </summary>
public class SoftRemoveRoleByIdValidator : AbstractValidator<SoftRemoveRoleByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveRoleByIdValidator"/> class.
    /// </summary>
    public SoftRemoveRoleByIdValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
