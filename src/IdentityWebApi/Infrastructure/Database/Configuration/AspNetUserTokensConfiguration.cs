using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace IdentityWebApi.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of AspNetUserTokens entity.
/// </summary>
public class AspNetUserTokensConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        builder.Property(prop => prop.Value)
            .HasMaxLength(1024);
    }
}