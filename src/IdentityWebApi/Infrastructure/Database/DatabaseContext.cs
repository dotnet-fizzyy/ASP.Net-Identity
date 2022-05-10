using IdentityWebApi.Core.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Reflection;

namespace IdentityWebApi.Infrastructure.Database;

/// <summary>
/// Database EF context.
/// </summary>
public sealed class DatabaseContext : IdentityDbContext<
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
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseContext"/> class.
    /// </summary>
    /// <param name="options"><see cref="DbContextOptions{T}"/>.</param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        this.Database.Migrate();
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
