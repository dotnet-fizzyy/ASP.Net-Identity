using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace IdentityWebApi.Core.Entities;

public class AppUser : IdentityUser<Guid>, IBaseUser
{
    public IList<AppUserRole> UserRoles { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}