using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<UserDto>> SignUpUser(UserRegistrationActionModel userModel);
    }
}