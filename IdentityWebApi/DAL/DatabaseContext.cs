using System;
using IdentityWebApi.DAL.Configuration;
using IdentityWebApi.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL
{
    public class DatabaseContext : IdentityDbContext<
        AppUser, 
        AppRole, 
        Guid,  
        IdentityUserClaim<Guid>, 
        AppUserRole, 
        IdentityUserLogin<Guid>, 
        IdentityRoleClaim<Guid>, 
        IdentityUserToken<Guid>
    >
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserRoleConfiguration());
            builder.ApplyConfiguration(new EmailTemplateConfiguration());
            
            base.OnModelCreating(builder);
        }
    }
}