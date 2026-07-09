namespace VikashERP.Mobile.State;

public class AppStateService
{
    private const string AuthTokenKey = "jwt_token";
    private const string RefreshTokenKey = "refreshToken";

    public async Task SetTokenAsync(string token)
    {
        await SecureStorage.SetAsync(AuthTokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(AuthTokenKey);
    }

    public async Task RemoveTokenAsync()
    {
        SecureStorage.Remove(AuthTokenKey);
    }

    public async Task SetRefreshTokenAsync(string refreshToken)
    {
        await SecureStorage.SetAsync(RefreshTokenKey, refreshToken);
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await SecureStorage.GetAsync(RefreshTokenKey);
    }

    public async Task RemoveRefreshTokenAsync()
    {
        SecureStorage.Remove(RefreshTokenKey);
    }

    public bool IsPinUnlocked { get; set; } = false;

    // --- Global Loader State ---
    public bool IsLoading { get; private set; }
    public string? LoadingText { get; private set; }
    public string? LoadingSubText { get; private set; }
    public VikashERP.Mobile.Components.Shared.AdvanceLoader.LoaderType LoadingType { get; private set; } = VikashERP.Mobile.Components.Shared.AdvanceLoader.LoaderType.ColorfulBars;

    public event Action? OnChange;

    public void ShowLoader(string? text = null, string? subText = null, VikashERP.Mobile.Components.Shared.AdvanceLoader.LoaderType type = VikashERP.Mobile.Components.Shared.AdvanceLoader.LoaderType.ColorfulBars)
    {
        IsLoading = true;
        LoadingText = text;
        LoadingSubText = subText;
        LoadingType = type;
        NotifyStateChanged();
    }

    public void HideLoader()
    {
        IsLoading = false;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    // --- User Profile State ---
    private string? _userProfilePictureUrl;
    
    public string? UserProfilePictureUrl 
    { 
        get
        {
            if (_userProfilePictureUrl == null)
            {
                _userProfilePictureUrl = Microsoft.Maui.Storage.Preferences.Default.Get("cached_profile_pic", (string?)null);
            }
            return _userProfilePictureUrl;
        }
        private set
        {
            _userProfilePictureUrl = value;
        }
    }
    
    public event Action<string?>? OnProfilePictureChanged;

    public void UpdateProfilePicture(string? newUrl)
    {
        UserProfilePictureUrl = newUrl;
        if (!string.IsNullOrEmpty(newUrl))
        {
            Microsoft.Maui.Storage.Preferences.Default.Set("cached_profile_pic", newUrl);
        }
        OnProfilePictureChanged?.Invoke(newUrl);
    }
}