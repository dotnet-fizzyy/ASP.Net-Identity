using IdentityWebApi.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityWebApi.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of AppUserRole entity.
/// </summary>
public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasQueryFilter(prop => !prop.IsDeleted);

        builder
            .HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .HasPrincipalKey(x => x.Id)
            .IsRequired();
        builder
            .HasOne(x => x.AppUser)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id)
            .IsRequired();
    }
}
