using IdentityWebApi.DAL.Constants;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Utilities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace IdentityWebApi.DAL.Configuration;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired();
        builder.Property(x => x.Layout)
            .IsRequired();
        builder.Property(x => x.CreationDate)
            .HasDefaultValueSql("getdate()");

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasData(new
        {
            Id = new Guid("f8fd1c61-584c-4c37-8be9-54d39dd1c92c"),
            Name = EmailNames.EmailConfirmationTemplate,
            Layout = TemplateReader.ReadTemplateFromFolder(EmailNames.EmailConfirmationTemplate),
            CreationDate = new DateTime(2021, 7, 4),
            IsDeleted = false
        });
    }
}
