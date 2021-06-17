using System.Threading.Tasks;
using IdentityWebApi.DAL.Entities;

namespace IdentityWebApi.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> UpdateUser(User user);
    }
}