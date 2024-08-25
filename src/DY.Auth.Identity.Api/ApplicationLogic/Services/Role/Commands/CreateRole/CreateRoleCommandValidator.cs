using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.CreateRole;

/// <summary>
/// <see cref="CreateRoleCommand"/> validator.
/// </summary>
public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleCommandValidator"/> class.
    /// </summary>
    public CreateRoleCommandValidator()
    {
        this.RuleFor(command => command.Name)
            .NotNull()
            .NotEmpty();
    }
}
