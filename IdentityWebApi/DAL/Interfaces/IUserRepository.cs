using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;

namespace IdentityWebApi.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<AppUser>
    {
        Task<ServiceResult<AppUser>> UpdateUserAsync(AppUser appUser);

        Task<ServiceResult<AppUser>> CreateUserAsync(AppUser appUser, string password, string role);

        Task<ServiceResult> RemoveUserAsync(Guid id);
    }
}