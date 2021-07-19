using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebApi.DAL.Entities
{
    public class AppRole : IdentityRole<Guid>, IBaseUser
    {
        public IList<AppUserRole> UserRoles { get; set; }

        public DateTime CreationDate { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}