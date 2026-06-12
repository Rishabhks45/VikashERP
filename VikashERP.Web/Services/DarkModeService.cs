using Microsoft.JSInterop;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class DarkModeService : IDarkModeService
{
    private const string StorageKey = "vikash-dark-mode";
    private readonly IJSRuntime _js;
    private bool _isDarkMode;
    private bool _initialized;

    public DarkModeService(IJSRuntime js) => _js = js;

    public bool IsDarkMode => _isDarkMode;

    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        try
        {
            var stored = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            _isDarkMode = stored == "true";
        }
        catch
        {
            _isDarkMode = false;
        }

        _initialized = true;
        OnChange?.Invoke();
    }

    public async Task SetDarkModeAsync(bool isDarkMode)
    {
        if (_isDarkMode == isDarkMode && _initialized)
            return;

        _isDarkMode = isDarkMode;
        _initialized = true;

        try
        {
            await _js.InvokeAsync<object?>("localStorage.setItem", StorageKey, isDarkMode ? "true" : "false");
        }
        catch
        {
            // Ignore storage errors (prerender / private mode).
        }

        OnChange?.Invoke();
    }

    public Task ToggleAsync() => SetDarkModeAsync(!_isDarkMode);
}
