using IdentityWebApi.Core.Results;
using System.Threading.Tasks;
using IdentityWebApi.Presentation.Models.Action;
using IdentityWebApi.Presentation.Models.DTO;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IAuthService
{
    Task<ServiceResult<(UserResultDto userDto, string token)>> SignUpUserAsync(UserRegistrationActionModel userModel);

    Task<ServiceResult<UserResultDto>> SignInUserAsync(UserSignInActionModel userModel);

    Task<ServiceResult> ConfirmUserEmailAsync(string email, string token);
}
