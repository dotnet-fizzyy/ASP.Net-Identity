using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;

namespace IdentityWebApi.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<AppUser>
    {
        Task<ServiceResult<AppUser>> GetUserWithRoles(Guid id);
        
        Task<ServiceResult<AppUser>> SignInUserAsync(string email, string password);

        Task<ServiceResult<AppUser>> ConfirmUserEmailAsync(string email, string token);
        
        Task<ServiceResult<AppUser>> UpdateUserAsync(AppUser appUser);

        Task<ServiceResult<(AppUser appUser, string token)>> CreateUserAsync(AppUser appUser, string password, string role,  bool confirmImmediately);

        Task<ServiceResult> RemoveUserAsync(Guid id);
    }
}