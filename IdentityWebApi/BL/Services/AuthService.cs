using System.Threading.Tasks;
using AutoMapper;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<(UserDto userDto, string token)>> SignUpUserAsync(UserRegistrationActionModel userModel)
        {
            var userEntity = _mapper.Map<AppUser>(userModel);
            
            var createdResult = await _userRepository.CreateUserAsync(userEntity, userModel.Password, userModel.Role, false);
            if (createdResult.ServiceResultType is not ServiceResultType.Success)
            {
                return new ServiceResult<(UserDto userDto, string token)>(createdResult.ServiceResultType, createdResult.Message);
            }

            return new ServiceResult<(UserDto userDto, string token)>(ServiceResultType.Success, (_mapper.Map<UserDto>(createdResult.Data.appUser),  createdResult.Data.token));
        }

        public async Task<ServiceResult> ConfirmUserEmailAsync(string email, string token)
        {
            var emailConfirmationResult = await _userRepository.ConfirmUserEmailAsync(email, token);

            return emailConfirmationResult;
        }
    }
}