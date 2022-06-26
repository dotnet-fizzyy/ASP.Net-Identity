using IdentityWebApi.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityWebApi.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of AppUser entity.
/// </summary>
public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasQueryFilter(prop => !prop.IsDeleted);

        builder.Property(prop => prop.CreationDate)
            .HasDefaultValueSql("getdate()");

        builder.Property(prop => prop.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(prop => prop.PasswordHash)
            .HasMaxLength(2048);

        builder.Property(prop => prop.SecurityStamp)
            .HasMaxLength(2048);

        builder.Property(prop => prop.ConcurrencyStamp)
            .HasMaxLength(2048);
    }
}
