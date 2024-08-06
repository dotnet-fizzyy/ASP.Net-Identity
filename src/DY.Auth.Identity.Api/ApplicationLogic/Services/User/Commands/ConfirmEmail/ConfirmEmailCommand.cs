using DY.Auth.Identity.Api.Core.Results;

using MediatR;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.ConfirmEmail;

public record ConfirmEmailCommand : IRequest<ServiceResult>
{
    /// <summary>
    /// Gets user email to confirm.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets confirmation token.
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmEmailCommand"/> class.
    /// </summary>
    /// <param name="email">User email to confirm.</param>
    /// <param name="token">Confirmation token.</param>
    public ConfirmEmailCommand(string email, string token)
    {
        this.Email = email;
        this.Token = token;
    }
}
