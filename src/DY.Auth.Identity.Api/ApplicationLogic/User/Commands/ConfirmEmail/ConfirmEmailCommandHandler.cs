using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Core.Utilities;

using MediatR;

using Microsoft.AspNetCore.Identity;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.User.Commands.ConfirmEmail;

/// <summary>
/// Confirm user email CQRS handler.
/// </summary>
/// <param name="userManager"><see cref="UserManager{T}"/>.</param>
public class ConfirmEmailCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ConfirmEmailCommand, ServiceResult>
{
    private readonly UserManager<AppUser> userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

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
                IdentityUtilities.ConcatenateIdentityErrorMessages(confirmationResult.Errors));
        }

        return new ServiceResult(ServiceResultType.Success);
    }
}
