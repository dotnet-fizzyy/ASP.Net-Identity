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
            .HasOne(prop => prop.Role)
            .WithMany(prop => prop.UserRoles)
            .HasForeignKey(prop => prop.RoleId)
            .HasPrincipalKey(prop => prop.Id)
            .IsRequired();

        builder
            .HasOne(prop => prop.AppUser)
            .WithMany(prop => prop.UserRoles)
            .HasForeignKey(prop => prop.UserId)
            .HasPrincipalKey(prop => prop.Id)
            .IsRequired();
    }
}
