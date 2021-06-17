using System;
using System.Threading.Tasks;
using IdentityWebApi.PL.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUser(Guid id);
        
        Task<UserDto> CreateUser(UserDto user);
        
        Task<UserDto> UpdateUser(UserDto user);
    }
}