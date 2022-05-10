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
        builder.Property(x => x.CreationDate)
            .HasDefaultValueSql("getdate()");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
