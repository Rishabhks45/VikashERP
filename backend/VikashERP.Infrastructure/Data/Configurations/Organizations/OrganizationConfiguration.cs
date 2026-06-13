using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Organizations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> entity)
    {

            entity.ToTable("Organizations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LegalName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Tagline).HasMaxLength(500);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.FaviconUrl).HasMaxLength(500);
            entity.Property(e => e.LoginBackgroundUrl).HasMaxLength(500);
            entity.Property(e => e.PrimaryColor).HasMaxLength(20);
            entity.Property(e => e.SecondaryColor).HasMaxLength(20);
            entity.Property(e => e.AddressLine1).HasMaxLength(255);
            entity.Property(e => e.AddressLine2).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.PinCode).HasMaxLength(20);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.WebsiteUrl).HasMaxLength(500);
            entity.Property(e => e.WhatsAppNumber).HasMaxLength(30);
            entity.Property(e => e.Gstin).HasMaxLength(15);
            entity.Property(e => e.Pan).HasMaxLength(10);
            entity.Property(e => e.BankName).HasMaxLength(255);
            entity.Property(e => e.BankAccountName).HasMaxLength(255);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.IfscCode).HasMaxLength(20);
            entity.Property(e => e.EmailFromName).HasMaxLength(255);
            entity.Property(e => e.EmailFromAddress).HasMaxLength(255);
            entity.Property(e => e.MetaTitle).HasMaxLength(255);
            entity.Property(e => e.MetaDescription).HasMaxLength(500);
            entity.Property(e => e.MetaKeywords).HasMaxLength(500);
            entity.Property(e => e.FooterText).HasMaxLength(1000);
            entity.Property(e => e.CopyrightText).HasMaxLength(500);
            entity.Property(e => e.SocialFacebookUrl).HasMaxLength(500);
            entity.Property(e => e.SocialInstagramUrl).HasMaxLength(500);
            entity.Property(e => e.SocialLinkedInUrl).HasMaxLength(500);
            entity.Property(e => e.SocialYoutubeUrl).HasMaxLength(500);
            entity.Property(e => e.DefaultCurrency).IsRequired().HasMaxLength(10);
            entity.Property(e => e.DefaultWeightUnit).IsRequired().HasMaxLength(10);
            entity.Property(e => e.TimeZone).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DateFormat).IsRequired().HasMaxLength(30);
    }
}
