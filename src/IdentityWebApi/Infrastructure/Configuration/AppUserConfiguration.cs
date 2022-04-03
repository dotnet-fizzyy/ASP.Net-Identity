using IdentityWebApi.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityWebApi.Infrastructure.Configuration;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.CreationDate)
            .HasDefaultValueSql("getdate()");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
