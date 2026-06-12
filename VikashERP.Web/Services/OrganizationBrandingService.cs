using System.Net.Http.Json;
using MudBlazor;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class OrganizationBrandingService : IOrganizationBrandingService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private OrganizationPublicModel _current = new();
    private bool _initialized;

    public OrganizationBrandingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public OrganizationPublicModel Current => _current;

    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        await RefreshAsync();
        _initialized = true;
    }

    public async Task RefreshAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("VikashERP.Api.Anonymous");
            var organization = await client.GetFromJsonAsync<OrganizationPublicModel>("api/organization/public");
            if (organization is not null)
                _current = organization;
        }
        catch
        {
            // UI keeps last loaded values; theme uses neutral fallbacks until API responds.
        }

        OnChange?.Invoke();
    }

    public MudTheme GetTheme() => VikashTheme.DefaultTheme;

    public string CssVariables =>
        $"--org-primary: {VikashTheme.ShellPrimary}; " +
        $"--org-secondary: {VikashTheme.ShellSecondary}; " +
        $"--org-accent: {VikashTheme.ShellAccent};";

    public string LogoSrc => ResolveAssetUrl(_current.LogoUrl);

    public string FaviconSrc => ResolveAssetUrl(_current.FaviconUrl);

    public string PageTitle
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_current.MetaTitle))
                return _current.MetaTitle.Trim();

            return string.IsNullOrWhiteSpace(_current.DisplayName)
                ? string.Empty
                : _current.DisplayName.Trim();
        }
    }

    public bool HasLoginBackgroundImage => !string.IsNullOrWhiteSpace(_current.LoginBackgroundUrl);

    public string AuthPanelStyle
    {
        get
        {
            if (HasLoginBackgroundImage)
                return $"{CssVariables} background-image: linear-gradient(135deg, rgba(15, 23, 42, 0.82), rgba(15, 23, 42, 0.92)), url('{_current.LoginBackgroundUrl}'); background-size: cover; background-position: center;";

            return $"{CssVariables} background: linear-gradient(160deg, {VikashTheme.ShellSurface} 0%, {VikashTheme.ShellSecondary} 50%, #0b1220 100%);";
        }
    }

    private static string ResolveAssetUrl(string? configuredUrl) =>
        string.IsNullOrWhiteSpace(configuredUrl) ? string.Empty : configuredUrl.Trim();
}
