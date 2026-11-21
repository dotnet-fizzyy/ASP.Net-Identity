using DY.Auth.Identity.Api.ApplicationLogic.Shared.Validators;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.RemoveRoleById;

/// <summary>
/// <see cref="RemoveRoleByIdCommand"/> validator.
/// </summary>
public class RemoveRoleByIdCommandValidator : AbstractValidator<RemoveRoleByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveRoleByIdCommandValidator"/> class.
    /// </summary>
    public RemoveRoleByIdCommandValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
