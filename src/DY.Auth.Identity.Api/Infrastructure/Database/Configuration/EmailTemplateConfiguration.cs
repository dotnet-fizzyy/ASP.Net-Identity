using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Utilities;
using DY.Auth.Identity.Api.Infrastructure.Database.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace DY.Auth.Identity.Api.Infrastructure.Database.Configuration;

/// <summary>
/// Configuration of EmailTemplate entity.
/// </summary>
public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasQueryFilter(prop => !prop.IsDeleted);

        builder.HasKey(prop => prop.Id);

        builder.Property(prop => prop.Subject)
            .HasMaxLength(256);

        builder.Property(prop => prop.Name)
            .IsRequired();

        builder.Property(prop => prop.Layout)
            .IsRequired()
            .HasColumnType("nvarchar(4000)");

        builder.Property(prop => prop.CreationDate)
            .HasDefaultValueSql("getdate()");

        builder.HasIndex(prop => prop.Name)
            .IsUnique();

        builder.HasData(new EmailTemplate
        {
            Id = EntityConfigurationConstants.EmailConfirmationTemplateId,
            Name = Templates.EmailConfirmationTemplate,
            Subject = EmailSubjects.EmailConfirmation,
            Layout = TemplateReader.ReadTemplateFromFolder(Templates.EmailConfirmationTemplate),
            CreationDate = new DateTime(2021, 7, 4),
            IsDeleted = false,
        });
    }
}
