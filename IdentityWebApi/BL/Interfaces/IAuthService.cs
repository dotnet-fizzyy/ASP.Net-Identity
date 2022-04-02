using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

using System.Threading.Tasks;

namespace IdentityWebApi.BL.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<(UserResultDto userDto, string token)>> SignUpUserAsync(UserRegistrationActionModel userModel);

    Task<ServiceResult<UserResultDto>> SignInUserAsync(UserSignInActionModel userModel);

    Task<ServiceResult> ConfirmUserEmailAsync(string email, string token);
}
