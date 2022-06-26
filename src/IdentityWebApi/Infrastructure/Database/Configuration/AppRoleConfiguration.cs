using IdentityWebApi.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityWebApi.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of AppRole entity.
/// </summary>
public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(x => x.CreationDate)
            .HasDefaultValueSql("getdate()");

        builder.Property(x => x.ConcurrencyStamp)
            .HasMaxLength(2048);
    }
}
