namespace VikashERP.Web.Services.Interfaces;

public interface IDarkModeService
{
    bool IsDarkMode { get; }

    event Action? OnChange;

    Task InitializeAsync();

    Task SetDarkModeAsync(bool isDarkMode);

    Task ToggleAsync();
}
