using System.Threading.Tasks;
using IdentityWebApi.DAL.Entities;

namespace IdentityWebApi.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<AppUser>
    {
        Task<AppUser> UpdateUser(AppUser appUser);
    }
}