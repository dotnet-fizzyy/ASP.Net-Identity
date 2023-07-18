using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.SoftRemoveRoleById;

/// <summary>
/// <see cref="SoftRemoveRoleByIdCommand"/> validator.
/// </summary>
public class SoftRemoveRoleByIdCommandValidator : AbstractValidator<SoftRemoveRoleByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveRoleByIdCommandValidator"/> class.
    /// </summary>
    public SoftRemoveRoleByIdCommandValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
