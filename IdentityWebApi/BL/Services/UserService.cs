using System;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWebApi.BL.Constants;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.PL.DTO;

namespace IdentityWebApi.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserDto>> GetUserAsync(Guid id)
        {
            var user = await _userRepository.SearchForSingleItemAsync(x => x.Id == id, inc => inc.UserRoles);
            if (user is null)
            {
                return new ServiceResult<UserDto>(ServiceResultType.NotFound, ExceptionMessageConstants.MissingUser);
            }

            return new ServiceResult<UserDto> 
            {
                ServiceResultType = ServiceResultType.Success,
                Data = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<ServiceResult<UserDto>> CreateUserAsync(UserDto user)
        {
            var userEntity = _mapper.Map<AppUser>(user);
            
            var createdUserResult = await _userRepository.CreateUserAsync(userEntity, user.Password, user.UserRole);
            
            return new ServiceResult<UserDto>
            {
                ServiceResultType = createdUserResult.ServiceResultType,
                Message = createdUserResult.Message,
                Data = createdUserResult.Data != null ? _mapper.Map<UserDto>(createdUserResult.Data) : default
            };
        }

        public async Task<ServiceResult<UserDto>> UpdateUserAsync(UserDto user)
        {
            var userEntity = _mapper.Map<AppUser>(user);

            var updatedUserResult = await _userRepository.UpdateUserAsync(userEntity);

            return new ServiceResult<UserDto>
            {
                ServiceResultType = updatedUserResult.ServiceResultType,
                Message = updatedUserResult.Message,
                Data = updatedUserResult.Data != null ? _mapper.Map<UserDto>(updatedUserResult.Data) : default
            };
        }

        public async Task<ServiceResult> RemoveUserAsync(Guid id)
        {
            return await _userRepository.RemoveUserAsync(id);
        }
    }
}