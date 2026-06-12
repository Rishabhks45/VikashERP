using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] private IDarkModeService DarkModeService { get; set; } = default!;
    [Inject] private IOrganizationBrandingService OrganizationBranding { get; set; } = default!;

    private bool _drawerOpen;
    private string _userName = "User";
    private string _userEmail = string.Empty;
    private string _userRole = string.Empty;
    private string _userInitials = "U";
    private string? _userProfilePictureUrl;
    private string _displayRole => string.IsNullOrWhiteSpace(_userRole) ? "Staff" : _userRole;

    protected override async Task OnInitializedAsync()
    {
        AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
        DarkModeService.OnChange += OnDarkModeChanged;
        OrganizationBranding.OnChange += OnBrandingChanged;
        await OrganizationBranding.InitializeAsync();
        await LoadUserAsync();
    }

    private void OnBrandingChanged() => InvokeAsync(StateHasChanged);

    private void OnDarkModeChanged() => InvokeAsync(StateHasChanged);

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        await LoadUserAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadUserAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            _userName = "User";
            _userEmail = string.Empty;
            _userRole = string.Empty;
            _userInitials = "U";
            _userProfilePictureUrl = null;
            return;
        }

        _userName = GetClaim(user, ClaimTypes.Name, "name") ?? "User";
        _userEmail = GetClaim(user, ClaimTypes.Email, JwtRegisteredClaimNames.Email, "email") ?? string.Empty;
        _userRole = user.FindFirst(ClaimTypes.Role)?.Value
            ?? user.FindFirst("roles")?.Value
            ?? string.Empty;
        _userInitials = GetInitials(_userName);
        _userProfilePictureUrl = GetClaim(user, "picture", ClaimTypes.Uri);
    }

    private static string? GetClaim(ClaimsPrincipal user, params string[] types)
    {
        foreach (var type in types)
        {
            var value = user.FindFirst(type)?.Value?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return null;
    }

    private static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "U";

        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
            return parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant();

        return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
    }

    private void DrawerToggle() => _drawerOpen = !_drawerOpen;

    private Task ToggleDarkMode() => DarkModeService.ToggleAsync();

    private void Logout() => Navigation.NavigateTo("/account/logout", forceLoad: true);

    public void Dispose()
    {
        AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        DarkModeService.OnChange -= OnDarkModeChanged;
        OrganizationBranding.OnChange -= OnBrandingChanged;
    }
}
