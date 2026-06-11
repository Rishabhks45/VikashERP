using MudBlazor;

namespace VikashERP.Web;

public static class VikashTheme
{
    public static MudTheme DefaultTheme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#6f42c1", // Deep purple from "SIGN IN" button
            Secondary = "#1e293b", // Navy sidebar color
            AppbarBackground = "#ffffff",
            AppbarText = "#424242",
            Background = "#f4f6f8",
            DrawerBackground = "#111827", // Deep navy for drawer
            DrawerText = "#e2e8f0",
            DrawerIcon = "#94a3b8"
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#8b5cf6",
            Secondary = "#0f172a",
            AppbarBackground = "#1e293b",
            Background = "#0f172a",
            DrawerBackground = "#0f172a",
            DrawerText = "#f8fafc",
            DrawerIcon = "#cbd5e1"
        },
        LayoutProperties = new LayoutProperties()
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "300px"
        }
    };
}
