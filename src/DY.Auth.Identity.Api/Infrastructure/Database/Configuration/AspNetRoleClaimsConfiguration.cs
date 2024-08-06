using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace DY.Auth.Identity.Api.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of AspNetRoleClaims entity.
/// </summary>
public class AspNetRoleClaimsConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.Property(prop => prop.ClaimType)
            .HasMaxLength(256);

        builder.Property(prop => prop.ClaimValue)
            .HasMaxLength(256);
    }
}
