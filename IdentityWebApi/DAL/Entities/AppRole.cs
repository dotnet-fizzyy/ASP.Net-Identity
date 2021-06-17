using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebApi.DAL.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public IList<AppUserRole> UserRoles { get; set; }
    }
}