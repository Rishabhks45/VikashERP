using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace VikashERP.Web.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    private bool _drawerOpen;
    private bool _isDarkMode;
    private string _userName = "User";
    private string _userEmail = string.Empty;
    private string _userRole = string.Empty;
    private string _userInitials = "U";

    protected override async Task OnInitializedAsync()
    {
        AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
        await LoadUserAsync();
    }

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
            return;
        }

        _userName = GetClaim(user, ClaimTypes.Name, "name") ?? "User";
        _userEmail = GetClaim(user, ClaimTypes.Email, JwtRegisteredClaimNames.Email, "email") ?? string.Empty;
        _userRole = user.FindFirst(ClaimTypes.Role)?.Value
            ?? user.FindFirst("roles")?.Value
            ?? string.Empty;
        _userInitials = GetInitials(_userName);
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

    private void ToggleDarkMode() => _isDarkMode = !_isDarkMode;

    private void NavigateToProfile() => Navigation.NavigateTo("/profile");

    private void Logout() => Navigation.NavigateTo("/account/logout", forceLoad: true);

    public void Dispose() =>
        AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
}
