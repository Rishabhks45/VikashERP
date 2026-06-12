namespace VikashERP.Web.Models;

/// <summary>
/// Public organization payload from GET /api/organization/public — all values come from the database.
/// </summary>
public class OrganizationPublicModel
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
    public string Country { get; set; } = string.Empty;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? FooterText { get; set; }
    public string? CopyrightText { get; set; }
    public string? SocialFacebookUrl { get; set; }
    public string? SocialInstagramUrl { get; set; }
    public string? SocialLinkedInUrl { get; set; }
    public string? SocialYoutubeUrl { get; set; }
    public string DefaultCurrency { get; set; } = string.Empty;
}
