using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace IdentityWebApi.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of AspNetUserClaims entity.
/// </summary>
public class AspNetUserClaimsConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.Property(prop => prop.ClaimType)
            .HasMaxLength(256);

        builder.Property(prop => prop.ClaimValue)
            .HasMaxLength(256);
    }
}