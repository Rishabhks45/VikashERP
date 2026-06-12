using VikashERP.Application.Features.Organization.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _repository;

    public OrganizationService(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrganizationDto> GetOrCreateAsync(CancellationToken cancellationToken = default)
    {
        var organization = await _repository.GetOrCreateDefaultAsync(cancellationToken);
        return MapToDto(organization);
    }

    public async Task<OrganizationPublicDto> GetPublicAsync(CancellationToken cancellationToken = default)
    {
        var organization = await _repository.GetOrCreateDefaultAsync(cancellationToken);
        return MapToPublicDto(organization);
    }

    public async Task<OrganizationDto?> UpdateAsync(UpdateOrganizationRequest request, CancellationToken cancellationToken = default)
    {
        var organization = await _repository.GetOrCreateDefaultAsync(cancellationToken);
        ApplyUpdate(organization, request);
        await _repository.UpdateAsync(organization, cancellationToken);
        return MapToDto(organization);
    }

    private static void ApplyUpdate(Organization organization, UpdateOrganizationRequest request)
    {
        organization.LegalName = request.LegalName.Trim();
        organization.DisplayName = request.DisplayName.Trim();
        organization.Tagline = NullIfWhiteSpace(request.Tagline);
        organization.LogoUrl = NullIfWhiteSpace(request.LogoUrl);
        organization.FaviconUrl = NullIfWhiteSpace(request.FaviconUrl);
        organization.LoginBackgroundUrl = NullIfWhiteSpace(request.LoginBackgroundUrl);
        organization.PrimaryColor = NullIfWhiteSpace(request.PrimaryColor);
        organization.SecondaryColor = NullIfWhiteSpace(request.SecondaryColor);
        organization.AddressLine1 = NullIfWhiteSpace(request.AddressLine1);
        organization.AddressLine2 = NullIfWhiteSpace(request.AddressLine2);
        organization.City = NullIfWhiteSpace(request.City);
        organization.State = NullIfWhiteSpace(request.State);
        organization.PinCode = NullIfWhiteSpace(request.PinCode);
        organization.Country = string.IsNullOrWhiteSpace(request.Country) ? "India" : request.Country.Trim();
        organization.Phone = NullIfWhiteSpace(request.Phone);
        organization.Email = NullIfWhiteSpace(request.Email);
        organization.WebsiteUrl = NullIfWhiteSpace(request.WebsiteUrl);
        organization.WhatsAppNumber = NullIfWhiteSpace(request.WhatsAppNumber);
        organization.Gstin = NullIfWhiteSpace(request.Gstin);
        organization.Pan = NullIfWhiteSpace(request.Pan);
        organization.BankName = NullIfWhiteSpace(request.BankName);
        organization.BankAccountName = NullIfWhiteSpace(request.BankAccountName);
        organization.BankAccountNumber = NullIfWhiteSpace(request.BankAccountNumber);
        organization.IfscCode = NullIfWhiteSpace(request.IfscCode);
        organization.EmailFromName = NullIfWhiteSpace(request.EmailFromName);
        organization.EmailFromAddress = NullIfWhiteSpace(request.EmailFromAddress);
        organization.MetaTitle = NullIfWhiteSpace(request.MetaTitle);
        organization.MetaDescription = NullIfWhiteSpace(request.MetaDescription);
        organization.MetaKeywords = NullIfWhiteSpace(request.MetaKeywords);
        organization.FooterText = NullIfWhiteSpace(request.FooterText);
        organization.CopyrightText = NullIfWhiteSpace(request.CopyrightText);
        organization.SocialFacebookUrl = NullIfWhiteSpace(request.SocialFacebookUrl);
        organization.SocialInstagramUrl = NullIfWhiteSpace(request.SocialInstagramUrl);
        organization.SocialLinkedInUrl = NullIfWhiteSpace(request.SocialLinkedInUrl);
        organization.SocialYoutubeUrl = NullIfWhiteSpace(request.SocialYoutubeUrl);
        organization.DefaultCurrency = string.IsNullOrWhiteSpace(request.DefaultCurrency) ? "INR" : request.DefaultCurrency.Trim();
        organization.DefaultWeightUnit = string.IsNullOrWhiteSpace(request.DefaultWeightUnit) ? "KG" : request.DefaultWeightUnit.Trim();
        organization.TimeZone = string.IsNullOrWhiteSpace(request.TimeZone) ? "Asia/Kolkata" : request.TimeZone.Trim();
        organization.DateFormat = string.IsNullOrWhiteSpace(request.DateFormat) ? "dd-MM-yyyy" : request.DateFormat.Trim();
        organization.EnableCustomerPortal = request.EnableCustomerPortal;
        organization.EnableLowStockAlerts = request.EnableLowStockAlerts;
        organization.EnablePaymentReminders = request.EnablePaymentReminders;
        organization.EnableDailyReportEmail = request.EnableDailyReportEmail;
        organization.EnableTradeConfirmations = request.EnableTradeConfirmations;
        organization.IsActive = request.IsActive;
    }

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static OrganizationDto MapToDto(Organization organization) => new()
    {
        Id = organization.Id,
        LegalName = organization.LegalName,
        DisplayName = organization.DisplayName,
        Tagline = organization.Tagline,
        LogoUrl = organization.LogoUrl,
        FaviconUrl = organization.FaviconUrl,
        LoginBackgroundUrl = organization.LoginBackgroundUrl,
        PrimaryColor = organization.PrimaryColor,
        SecondaryColor = organization.SecondaryColor,
        AddressLine1 = organization.AddressLine1,
        AddressLine2 = organization.AddressLine2,
        City = organization.City,
        State = organization.State,
        PinCode = organization.PinCode,
        Country = organization.Country,
        Phone = organization.Phone,
        Email = organization.Email,
        WebsiteUrl = organization.WebsiteUrl,
        WhatsAppNumber = organization.WhatsAppNumber,
        Gstin = organization.Gstin,
        Pan = organization.Pan,
        BankName = organization.BankName,
        BankAccountName = organization.BankAccountName,
        BankAccountNumber = organization.BankAccountNumber,
        IfscCode = organization.IfscCode,
        EmailFromName = organization.EmailFromName,
        EmailFromAddress = organization.EmailFromAddress,
        MetaTitle = organization.MetaTitle,
        MetaDescription = organization.MetaDescription,
        MetaKeywords = organization.MetaKeywords,
        FooterText = organization.FooterText,
        CopyrightText = organization.CopyrightText,
        SocialFacebookUrl = organization.SocialFacebookUrl,
        SocialInstagramUrl = organization.SocialInstagramUrl,
        SocialLinkedInUrl = organization.SocialLinkedInUrl,
        SocialYoutubeUrl = organization.SocialYoutubeUrl,
        DefaultCurrency = organization.DefaultCurrency,
        DefaultWeightUnit = organization.DefaultWeightUnit,
        TimeZone = organization.TimeZone,
        DateFormat = organization.DateFormat,
        EnableCustomerPortal = organization.EnableCustomerPortal,
        EnableLowStockAlerts = organization.EnableLowStockAlerts,
        EnablePaymentReminders = organization.EnablePaymentReminders,
        EnableDailyReportEmail = organization.EnableDailyReportEmail,
        EnableTradeConfirmations = organization.EnableTradeConfirmations,
        IsActive = organization.IsActive,
        UpdatedAt = organization.UpdatedAt
    };

    private static OrganizationPublicDto MapToPublicDto(Organization organization) => new()
    {
        DisplayName = organization.DisplayName,
        Tagline = organization.Tagline,
        LogoUrl = organization.LogoUrl,
        FaviconUrl = organization.FaviconUrl,
        LoginBackgroundUrl = organization.LoginBackgroundUrl,
        PrimaryColor = organization.PrimaryColor,
        SecondaryColor = organization.SecondaryColor,
        Phone = organization.Phone,
        Email = organization.Email,
        WebsiteUrl = organization.WebsiteUrl,
        WhatsAppNumber = organization.WhatsAppNumber,
        AddressLine1 = organization.AddressLine1,
        City = organization.City,
        State = organization.State,
        PinCode = organization.PinCode,
        Country = organization.Country,
        MetaTitle = organization.MetaTitle,
        MetaDescription = organization.MetaDescription,
        FooterText = organization.FooterText,
        CopyrightText = organization.CopyrightText,
        SocialFacebookUrl = organization.SocialFacebookUrl,
        SocialInstagramUrl = organization.SocialInstagramUrl,
        SocialLinkedInUrl = organization.SocialLinkedInUrl,
        SocialYoutubeUrl = organization.SocialYoutubeUrl,
        DefaultCurrency = organization.DefaultCurrency
    };
}
