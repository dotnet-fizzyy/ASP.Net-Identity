using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IAuthService
{
    Task<ServiceResult<(UserResultDto userDto, string token)>> SignUpUserAsync(UserRegistrationDto userModel);

    Task<ServiceResult<UserResultDto>> SignInUserAsync(UserSignInDto userModel);

    Task<ServiceResult> ConfirmUserEmailAsync(string email, string token);
}
