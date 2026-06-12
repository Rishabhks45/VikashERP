using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
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
            }

            if (!string.IsNullOrEmpty(_userEmail))
            {
                var key = $"user_profile_{_userEmail}";
                var storedJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", key);
                if (!string.IsNullOrEmpty(storedJson))
                {
                    var deserialized = System.Text.Json.JsonSerializer.Deserialize<UserProfileDto>(storedJson);
                    if (deserialized != null)
                    {
                        if (!string.IsNullOrEmpty(deserialized.FirstName) || !string.IsNullOrEmpty(deserialized.LastName))
                        {
                            _userName = $"{deserialized.FirstName} {deserialized.LastName}".Trim();
                            _userInitials = GetInitials(_userName);
                        }
                        if (!string.IsNullOrEmpty(deserialized.ProfilePictureUrl))
                        {
                            _userProfilePictureUrl = deserialized.ProfilePictureUrl;
                        }
                        StateHasChanged();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user profile from storage: {ex.Message}");
        }
    }

    private void HandleProfileUpdated(UserProfileDto profile)
    {
        if (profile != null && profile.Email == _userEmail)
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

    public void Dispose()
    {
        AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        DarkModeService.OnChange -= OnDarkModeChanged;
        OrganizationBranding.OnChange -= OnBrandingChanged;
        UserProfileService.OnProfileUpdated -= HandleProfileUpdated;
    }
}
