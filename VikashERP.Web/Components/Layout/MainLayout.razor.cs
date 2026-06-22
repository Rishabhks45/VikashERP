using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] private IDarkModeService DarkModeService { get; set; } = default!;
    [Inject] private IOrganizationBrandingService OrganizationBranding { get; set; } = default!;
    [Inject] private IUserProfileService UserProfileService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private bool _drawerOpen;
    private string _userName = "User";
    private string _userEmail = string.Empty;
    private string _userRole = string.Empty;
    private string _userInitials = "U";
    private Guid? _userId;
    private string? _userProfilePictureUrl;
    private string _displayRole => string.IsNullOrWhiteSpace(_userRole) ? "Staff" : _userRole;

    protected override async Task OnInitializedAsync()
    {
        AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
        DarkModeService.OnChange += OnDarkModeChanged;
        OrganizationBranding.OnChange += OnBrandingChanged;
        UserProfileService.OnProfileUpdated += HandleProfileUpdated;
        await OrganizationBranding.InitializeAsync();
        await LoadUserAsync();
    }

    private void OnBrandingChanged() => InvokeAsync(StateHasChanged);

    private void OnDarkModeChanged() => InvokeAsync(StateHasChanged);

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        await LoadUserAsync();
        await LoadUserProfileFromStorageAsync();
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
        
        var userIdStr = GetClaim(user, ClaimTypes.NameIdentifier, JwtRegisteredClaimNames.Sub, "sub");
        if (Guid.TryParse(userIdStr, out var uid))
            _userId = uid;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadUserProfileFromStorageAsync();
        }
    }

    private async Task LoadUserProfileFromStorageAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_userEmail))
            {
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                _userEmail = GetClaim(user, ClaimTypes.Email, JwtRegisteredClaimNames.Email, "email") ?? string.Empty;
                
                var userIdStr = GetClaim(user, ClaimTypes.NameIdentifier, JwtRegisteredClaimNames.Sub, "sub");
                if (Guid.TryParse(userIdStr, out var uid))
                    _userId = uid;
            }

            if (_userId.HasValue)
            {
                var profile = await UserProfileService.GetProfileAsync(_userId.Value);
                if (profile != null)
                {
                    if (!string.IsNullOrEmpty(profile.FirstName) || !string.IsNullOrEmpty(profile.LastName))
                    {
                        _userName = $"{profile.FirstName} {profile.LastName}".Trim();
                        _userInitials = GetInitials(_userName);
                    }
                    if (!string.IsNullOrEmpty(profile.ProfilePictureUrl))
                    {
                        _userProfilePictureUrl = profile.ProfilePictureUrl;
                    }
                    StateHasChanged();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user profile from server: {ex.Message}");
        }
    }

    private void HandleProfileUpdated(UserProfileDto profile)
    {
        if (profile != null)
        {
            _userName = $"{profile.FirstName} {profile.LastName}".Trim();
            if (string.IsNullOrEmpty(_userName))
            {
                _userName = profile.Email;
            }
            _userInitials = GetInitials(_userName);
            _userProfilePictureUrl = profile.ProfilePictureUrl;
            InvokeAsync(StateHasChanged);
        }
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

    // Customer storefront navigation helpers
    private string _globalSearchQuery = string.Empty;

    private void OnSearchKeyUp(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            Navigation.NavigateTo($"/customer-dashboard?search={Uri.EscapeDataString(_globalSearchQuery)}");
        }
    }

    private void ResetSearchAndGoHome()
    {
        _globalSearchQuery = string.Empty;
        Navigation.NavigateTo("/customer-dashboard");
    }

    private void FilterByCategory(string category)
    {
        _globalSearchQuery = category;
        Navigation.NavigateTo($"/customer-dashboard?search={Uri.EscapeDataString(category)}");
    }

    private void RedirectToRfq()
    {
        Navigation.NavigateTo("/customer-dashboard?tab=rfq");
    }

    private bool IsActiveLink(string path, string? tab)
    {
        var uri = Navigation.Uri;
        if (path.Equals("/customer-dashboard", StringComparison.OrdinalIgnoreCase))
        {
            // Parse base path (ignoring query strings)
            var absUri = Navigation.ToAbsoluteUri(uri);
            var isHomePath = absUri.AbsolutePath.Equals("/", StringComparison.OrdinalIgnoreCase) || 
                             absUri.AbsolutePath.Equals("/customer-dashboard", StringComparison.OrdinalIgnoreCase);
            
            if (string.IsNullOrEmpty(tab))
            {
                return isHomePath && !uri.Contains("tab=");
            }
            else
            {
                return isHomePath && uri.Contains($"tab={tab}", StringComparison.OrdinalIgnoreCase);
            }
        }
        
        var checkPath = Navigation.ToAbsoluteUri(uri).AbsolutePath;
        return checkPath.Equals(path, StringComparison.OrdinalIgnoreCase);
    }

    public void Dispose()
    {
        AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        DarkModeService.OnChange -= OnDarkModeChanged;
        OrganizationBranding.OnChange -= OnBrandingChanged;
        UserProfileService.OnProfileUpdated -= HandleProfileUpdated;
    }
}
