using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace IdentityWebApi.DAL.Entities;

public class AppRole : IdentityRole<Guid>, IBaseUser
{
    public IList<AppUserRole> UserRoles { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}
