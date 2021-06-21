using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

            var userDtoModel = createdResult.Data.appUser is not null 
                ? _mapper.Map<UserDto>(createdResult.Data.appUser) 
                : default;
            
            return new ServiceResult<(UserDto userDto, string token)>(
                createdResult.ServiceResultType,  
                createdResult.Message, 
                (userDtoModel,  createdResult.Data.token)
            );
        }

        public async Task<ServiceResult<UserDto>> SignInUserAsync(UserSignInActionModel userModel)
        {
            var signInResult = await _userRepository.SignInUserAsync(userModel.Email, userModel.Password);

            var userDtoModel = signInResult.Data is not null 
                ? _mapper.Map<UserDto>(signInResult.Data) 
                : default;
            userDtoModel.UserRole = signInResult.Data.UserRoles.FirstOrDefault().AppRole.Name;
            return new ServiceResult<UserDto>(
                signInResult.ServiceResultType, 
                signInResult.Message, 
                userDtoModel
            );
        }

        public async Task<ServiceResult> ConfirmUserEmailAsync(string email, string token)
        {
            var emailConfirmationResult = await _userRepository.ConfirmUserEmailAsync(email, token);

            return emailConfirmationResult;
        }
    }
}