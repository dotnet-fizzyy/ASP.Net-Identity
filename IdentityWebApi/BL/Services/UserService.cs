using System;
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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserResultDto>> GetUserAsync(Guid id)
        {
            var searchUserResult = await _userRepository.GetUserWithRoles(id);
            
            var userDtoModel = searchUserResult.Data is not null 
                ? _mapper.Map<UserResultDto>(searchUserResult.Data) 
                : default;

            return new ServiceResult<UserResultDto>(
                searchUserResult.Result, 
                searchUserResult.Message,
                userDtoModel
            );
        }

        public async Task<ServiceResult<UserResultDto>> CreateUserAsync(UserActionModel user)
        {
            var userEntity = _mapper.Map<AppUser>(user);
            
            var createdUserResult = await _userRepository.CreateUserAsync(userEntity, user.Password, user.UserRole, true);

            var userDtoModel = createdUserResult.Data.appUser is not null 
                ? _mapper.Map<UserResultDto>(createdUserResult.Data.appUser) 
                : default;
            
            return new ServiceResult<UserResultDto>(
                createdUserResult.Result, 
                createdUserResult.Message,
                userDtoModel
            );
        }

        public async Task<ServiceResult<UserResultDto>> UpdateUserAsync(UserActionModel user)
        {
            var userEntity = _mapper.Map<AppUser>(user);

            var updatedUserResult = await _userRepository.UpdateUserAsync(userEntity);

            var userDtoModel = updatedUserResult.Data is not null 
                ? _mapper.Map<UserResultDto>(updatedUserResult.Data) 
                : default;
            
            return new ServiceResult<UserResultDto>(
                updatedUserResult.Result,
                updatedUserResult.Message,
                userDtoModel
            );
        }

        public async Task<ServiceResult> RemoveUserAsync(Guid id) => 
            await _userRepository.RemoveUserAsync(id);
    }
}