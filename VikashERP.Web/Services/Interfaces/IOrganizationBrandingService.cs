using MudBlazor;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface IOrganizationBrandingService
{
    OrganizationPublicModel Current { get; }

    event Action? OnChange;

    Task InitializeAsync();

    Task RefreshAsync();

    MudTheme GetTheme();

    string CssVariables { get; }

    string LogoSrc { get; }

    string FaviconSrc { get; }

    string PageTitle { get; }

    string AuthPanelStyle { get; }

    bool HasLoginBackgroundImage { get; }
}
