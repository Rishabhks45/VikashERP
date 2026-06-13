using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.EmailTemplates;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> entity)
    {

            entity.ToTable("email_templates");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TemplateKey).HasColumnName("template_key").IsRequired().HasMaxLength(50);
            entity.Property(e => e.NotificationType).HasColumnName("notification_type").IsRequired();
            entity.Property(e => e.DisplayName).HasColumnName("display_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description").IsRequired().HasMaxLength(500);
            entity.Property(e => e.Subject).HasColumnName("subject").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Headline).HasColumnName("headline").IsRequired().HasMaxLength(255);
            entity.Property(e => e.BodyHtml).HasColumnName("body_html").IsRequired();
            entity.Property(e => e.Preheader).HasColumnName("preheader").HasMaxLength(255);
            entity.Property(e => e.ButtonLabel).HasColumnName("button_label").HasMaxLength(100);
            entity.Property(e => e.ButtonLinkToken).HasColumnName("button_link_token").HasMaxLength(100);
            entity.Property(e => e.AvailableTokens).HasColumnName("available_tokens").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(e => new { e.TemplateKey, e.NotificationType }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}
