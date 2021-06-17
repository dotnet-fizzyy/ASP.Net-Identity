using System.Threading.Tasks;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.DAL.Utilities;
using IdentityWebApi.Startup.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL.Repository
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppSettings _appSettings;
        
        public UserRepository(DatabaseContext databaseContext, UserManager<AppUser> userManager, AppSettings appSettings) : base(databaseContext)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }

        public async Task<AppUser> UpdateUser(AppUser appUser)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == appUser.Id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Id = appUser.Id;
            existingUser.Email = appUser.Email;
            existingUser.PhoneNumber = appUser.PhoneNumber;
            
            databaseContext.Update(existingUser);

            return appUser;
        }

        public override async Task<AppUser> CreateItemAsync(AppUser appUser)
        {
            if (!DatabaseUtilities.RoleExists(_appSettings.IdentitySettings.Roles, ""))
            {
                
            }
            
            var result = await _userManager.CreateAsync(appUser, appUser.PasswordHash);
            if (!result.Succeeded)
            {
                
            }
            
            await _userManager.AddToRoleAsync(appUser, "");
            
            return appUser;
        }
    }
}