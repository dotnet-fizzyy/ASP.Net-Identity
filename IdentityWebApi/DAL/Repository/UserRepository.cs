using System.Threading.Tasks;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.DAL.Utilities;
using IdentityWebApi.Startup.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppSettings _appSettings;
        
        public UserRepository(DatabaseContext databaseContext, UserManager<User> userManager, AppSettings appSettings) : base(databaseContext)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }

        public async Task<User> UpdateUser(User user)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Id = user.Id;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            
            databaseContext.Update(existingUser);

            return user;
        }

        public override async Task<User> CreateItemAsync(User user)
        {
            if (!DatabaseUtilities.RoleExists(_appSettings.IdentitySettings.Roles, ""))
            {
                
            }
            
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
            {
                
            }
            
            await _userManager.AddToRoleAsync(user, "");
            
            return user;
        }
    }
}