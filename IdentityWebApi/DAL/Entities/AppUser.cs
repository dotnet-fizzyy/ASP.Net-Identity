using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebApi.DAL.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public IList<AppUserRole> UserRoles { get; set; }
    }
}