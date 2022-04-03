using Microsoft.AspNetCore.Identity;

using System;

namespace IdentityWebApi.Core.Entities;

public class AppUserRole : IdentityUserRole<Guid>
{
    public AppUser AppUser { get; set; }

    public AppRole Role { get; set; }
}
