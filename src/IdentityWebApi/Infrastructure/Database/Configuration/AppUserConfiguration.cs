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
        builder.Property(x => x.CreationDate)
            .HasDefaultValueSql("getdate()");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
