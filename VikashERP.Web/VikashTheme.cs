using MudBlazor;

namespace VikashERP.Web;

public static class VikashTheme
{
    /// <summary>Fixed app shell palette (login panel, sidebar accents, MudBlazor theme).</summary>
    public const string ShellPrimary = "#6366f1";
    public const string ShellSecondary = "#0f172a";
    public const string ShellSurface = "#1e293b";
    public const string ShellAccent = "#818cf8";

    public static MudTheme DefaultTheme => CreateTheme(ShellPrimary, ShellSecondary);

    public static MudTheme CreateTheme(string? primaryColor, string? secondaryColor)
    {
        var primary = NormalizeHex(primaryColor, ShellPrimary);
        var secondary = NormalizeHex(secondaryColor, ShellSecondary);
        var primaryDark = Lighten(primary, 0.22);

        return new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = primary,
                Secondary = secondary,
                AppbarBackground = "#ffffff",
                AppbarText = "#334155",
                Background = "#f8fafc",
                Surface = "#ffffff",
                TextPrimary = "#0f172a",
                TextSecondary = "#64748b",
                Divider = "#e2e8f0",
                LinesDefault = "#e2e8f0",
                TableLines = "#e2e8f0",
                DrawerBackground = secondary,
                DrawerText = "#e2e8f0",
                DrawerIcon = "#94a3b8",
                ActionDefault = "#64748b"
            },
            PaletteDark = new PaletteDark
            {
                Primary = primaryDark,
                Secondary = "#94a3b8",
                AppbarBackground = secondary,
                AppbarText = "#f1f5f9",
                Background = "#0b1220",
                Surface = secondary,
                TextPrimary = "#f8fafc",
                TextSecondary = "#94a3b8",
                Divider = "#334155",
                LinesDefault = "#334155",
                TableLines = "#334155",
                DrawerBackground = "#0b1220",
                DrawerText = "#e2e8f0",
                DrawerIcon = "#94a3b8",
                ActionDefault = "#cbd5e1"
            },
            LayoutProperties = new LayoutProperties
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "300px"
            }
        };
    }

    private static string NormalizeHex(string? value, string fallback) =>
        string.IsNullOrWhiteSpace(value) || !value.TrimStart().StartsWith('#')
            ? fallback
            : value.Trim();

    private static string Lighten(string hex, double amount)
    {
        if (!TryParseHex(hex, out var r, out var g, out var b))
            return "#a78bfa";

        r = (int)Math.Min(255, r + 255 * amount);
        g = (int)Math.Min(255, g + 255 * amount);
        b = (int)Math.Min(255, b + 255 * amount);
        return $"#{r:x2}{g:x2}{b:x2}";
    }

    private static bool TryParseHex(string hex, out int r, out int g, out int b)
    {
        r = g = b = 0;
        hex = hex.TrimStart('#');
        if (hex.Length != 6)
            return false;

        return int.TryParse(hex[..2], System.Globalization.NumberStyles.HexNumber, null, out r)
            && int.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out g)
            && int.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out b);
    }
}
