using System;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebApi.DAL.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public AppUser AppUser { get; set; }
        
        public AppRole Role { get; set; }
    }
}