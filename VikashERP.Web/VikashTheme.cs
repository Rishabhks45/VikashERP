using MudBlazor;

namespace VikashERP.Web;

public static class VikashTheme
{
    public static MudTheme DefaultTheme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#6f42c1",
            Secondary = "#1e293b",
            AppbarBackground = "#ffffff",
            AppbarText = "#334155",
            Background = "#f8fafc",
            Surface = "#ffffff",
            TextPrimary = "#0f172a",
            TextSecondary = "#64748b",
            Divider = "#e2e8f0",
            LinesDefault = "#e2e8f0",
            TableLines = "#e2e8f0",
            DrawerBackground = "#0f172a",
            DrawerText = "#e2e8f0",
            DrawerIcon = "#94a3b8",
            ActionDefault = "#64748b"
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#a78bfa",
            Secondary = "#94a3b8",
            AppbarBackground = "#1e293b",
            AppbarText = "#f1f5f9",
            Background = "#0b1220",
            Surface = "#1e293b",
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
        LayoutProperties = new LayoutProperties()
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "300px"
        }
    };
}
