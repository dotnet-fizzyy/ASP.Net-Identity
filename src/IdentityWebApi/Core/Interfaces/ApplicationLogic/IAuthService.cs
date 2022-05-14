using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

/// <summary>
/// Authentication service.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Performs user sign-up.
    /// </summary>
    /// <param name="userModel"><see cref="UserRegistrationDto"/>.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with created user and confirmation email token.</returns>
    Task<ServiceResult<(UserResultDto userDto, string token)>> SignUpUserAsync(UserRegistrationDto userModel);

    /// <summary>
    /// Performs user sign-up.
    /// </summary>
    /// <param name="userModel"><see cref="UserSignInDto"/>.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with user.</returns>
    Task<ServiceResult<UserResultDto>> SignInUserAsync(UserSignInDto userModel);

    /// <summary>
    /// Confirms user email.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="token">Confirmation token.</param>
    /// <returns>A <see cref="ServiceResult"/> with result of confirmed user.</returns>
    Task<ServiceResult> ConfirmUserEmailAsync(string email, string token);
}
