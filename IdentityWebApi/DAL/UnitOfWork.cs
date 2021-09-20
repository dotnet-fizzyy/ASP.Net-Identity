using System;
using System.Threading.Tasks;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.DAL.Repository;
using IdentityWebApi.Startup.Settings;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebApi.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _databaseContext;
        
        public IUserRepository UserRepository { get; }
        
        public IRoleRepository RoleRepository { get; }
        
        public IEmailTemplateRepository EmailTemplateRepository { get; }

        public UnitOfWork(DatabaseContext databaseContext, 
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, 
            AppSettings appSettings
            )
        {
            _databaseContext = databaseContext;
            
            UserRepository = new UserRepository(databaseContext, userManager, signInManager, appSettings);
            RoleRepository = new RoleRepository(databaseContext);
            EmailTemplateRepository = new EmailTemplateRepository(databaseContext);
        }
        
        public async Task CommitAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _databaseContext.Dispose();
        }
    }
}