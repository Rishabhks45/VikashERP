namespace VikashERP.Application.Features.Organization.DTOs;

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string LegalName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Tagline { get; set; }
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? LoginBackgroundUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
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
    public string? Gstin { get; set; }
    public string? Pan { get; set; }
    public string? BankName { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? IfscCode { get; set; }
    public string? EmailFromName { get; set; }
    public string? EmailFromAddress { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? FooterText { get; set; }
    public string? CopyrightText { get; set; }
    public string? SocialFacebookUrl { get; set; }
    public string? SocialInstagramUrl { get; set; }
    public string? SocialLinkedInUrl { get; set; }
    public string? SocialYoutubeUrl { get; set; }
    public string DefaultCurrency { get; set; } = "INR";
    public string DefaultWeightUnit { get; set; } = "KG";
    public string TimeZone { get; set; } = "Asia/Kolkata";
    public string DateFormat { get; set; } = "dd-MM-yyyy";
    public bool EnableCustomerPortal { get; set; }
    public bool EnableLowStockAlerts { get; set; }
    public bool EnablePaymentReminders { get; set; }
    public bool EnableDailyReportEmail { get; set; }
    public bool EnableTradeConfirmations { get; set; }
    public bool IsActive { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class OrganizationPublicDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string? Tagline { get; set; }
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? LoginBackgroundUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? WhatsAppNumber { get; set; }
    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string Country { get; set; } = "India";
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? FooterText { get; set; }
    public string? CopyrightText { get; set; }
    public string? SocialFacebookUrl { get; set; }
    public string? SocialInstagramUrl { get; set; }
    public string? SocialLinkedInUrl { get; set; }
    public string? SocialYoutubeUrl { get; set; }
    public string DefaultCurrency { get; set; } = "INR";
}

public class UpdateOrganizationRequest
{
    public string LegalName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Tagline { get; set; }
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? LoginBackgroundUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
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
    public string? Gstin { get; set; }
    public string? Pan { get; set; }
    public string? BankName { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? IfscCode { get; set; }
    public string? EmailFromName { get; set; }
    public string? EmailFromAddress { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? FooterText { get; set; }
    public string? CopyrightText { get; set; }
    public string? SocialFacebookUrl { get; set; }
    public string? SocialInstagramUrl { get; set; }
    public string? SocialLinkedInUrl { get; set; }
    public string? SocialYoutubeUrl { get; set; }
    public string DefaultCurrency { get; set; } = "INR";
    public string DefaultWeightUnit { get; set; } = "KG";
    public string TimeZone { get; set; } = "Asia/Kolkata";
    public string DateFormat { get; set; } = "dd-MM-yyyy";
    public bool EnableCustomerPortal { get; set; }
    public bool EnableLowStockAlerts { get; set; } = true;
    public bool EnablePaymentReminders { get; set; } = true;
    public bool EnableDailyReportEmail { get; set; }
    public bool EnableTradeConfirmations { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
