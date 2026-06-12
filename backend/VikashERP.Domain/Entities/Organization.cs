namespace VikashERP.Domain.Entities;

/// <summary>
/// Single-tenant organization / website configuration (company profile, branding, regional & notification settings).
/// </summary>
public class Organization
{
    public int Id { get; set; } = 1;

    // Identity & branding
    public string LegalName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Tagline { get; set; }
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? LoginBackgroundUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }

    // Contact & address
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string Country { get; set; } = "India";
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? WhatsAppNumber { get; set; }

    // Tax & banking (invoices / compliance)
    public string? Gstin { get; set; }
    public string? Pan { get; set; }
    public string? BankName { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? IfscCode { get; set; }

    // Email identity (overrides appsettings when set)
    public string? EmailFromName { get; set; }
    public string? EmailFromAddress { get; set; }

    // Website SEO & footer
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? FooterText { get; set; }
    public string? CopyrightText { get; set; }

    // Social links
    public string? SocialFacebookUrl { get; set; }
    public string? SocialInstagramUrl { get; set; }
    public string? SocialLinkedInUrl { get; set; }
    public string? SocialYoutubeUrl { get; set; }

    // Regional defaults
    public string DefaultCurrency { get; set; } = "INR";
    public string DefaultWeightUnit { get; set; } = "KG";
    public string TimeZone { get; set; } = "Asia/Kolkata";
    public string DateFormat { get; set; } = "dd-MM-yyyy";

    // Feature toggles
    public bool EnableCustomerPortal { get; set; }
    public bool EnableLowStockAlerts { get; set; } = true;
    public bool EnablePaymentReminders { get; set; } = true;
    public bool EnableDailyReportEmail { get; set; }
    public bool EnableTradeConfirmations { get; set; } = true;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
