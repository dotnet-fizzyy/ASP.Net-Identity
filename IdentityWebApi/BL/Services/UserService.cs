using System;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWebApi.BL.Interfaces;
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

        public async Task<UserDto> GetUser(Guid id)
        {
            var user = await _userRepository.SearchForSingleItemAsync(x => x.Id == id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUser(UserDto user)
        {
            var userEntity = _mapper.Map<AppUser>(user);
            
            var createdEntity = await _userRepository.CreateItemAsync(userEntity);
            
            return _mapper.Map<UserDto>(createdEntity);
        }

        public async Task<UserDto> UpdateUser(UserDto user)
        {
            var userEntity = _mapper.Map<AppUser>(user);

            var updateUser = await _userRepository.UpdateUser(userEntity);

            return _mapper.Map<UserDto>(updateUser);
        }
    }
}