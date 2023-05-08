using FluentValidation;

using IdentityWebApi.ApplicationLogic.Services.Common;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.SoftRemoveUserById;

/// <summary>
/// <see cref="SoftRemoveUserByIdCommand"/> validator.
/// </summary>
public class SoftRemoveUserByIdCommandHandlerValidator : AbstractValidator<SoftRemoveUserByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftRemoveUserByIdCommandHandlerValidator"/> class.
    /// </summary>
    public SoftRemoveUserByIdCommandHandlerValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
