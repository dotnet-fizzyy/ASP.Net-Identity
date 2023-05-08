using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace IdentityWebApi.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of AspNetUserLogins entity.
/// </summary>
public class AspNetUserLoginsConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.Property(prop => prop.ProviderDisplayName)
            .HasMaxLength(512);
    }
}
