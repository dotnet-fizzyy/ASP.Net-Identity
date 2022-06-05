using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Core.Utilities;

using MediatR;

using Microsoft.AspNetCore.Identity;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.ConfirmEmail;

/// <summary>
/// Confirm user email CQRS handler.
/// </summary>
public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ServiceResult>
{
    private readonly UserManager<AppUser> userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmEmailCommandHandler"/> class.
    /// </summary>
    /// <param name="userManager"><see cref="UserManager{T}"/>.</param>
    public ConfirmEmailCommandHandler(UserManager<AppUser> userManager)
    {
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<ServiceResult> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            return new ServiceResult(ServiceResultType.NotFound);
        }

        var confirmationResult = await this.userManager.ConfirmEmailAsync(user, command.Token);

        if (!confirmationResult.Succeeded)
        {
            return new ServiceResult(
                ServiceResultType.InvalidData,
                IdentityUtilities.ConcatenateIdentityErrorMessages(confirmationResult.Errors)
            );
        }

        return new ServiceResult(ServiceResultType.Success);
    }
}