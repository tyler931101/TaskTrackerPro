using System.Windows;
using System.Windows.Media;

namespace TicketManagementSystem.Core;

public static class ThemeManager
{
    private static bool _isDark = false;

    public static void ToggleTheme()
    {
        var app = Application.Current;
        if (app == null) return;

        string themeFile = _isDark ? "/Resources/LightTheme.xaml" : "/Resources/DarkTheme.xaml";
        _isDark = !_isDark;

        var dict = new ResourceDictionary { Source = new System.Uri(themeFile, System.UriKind.Relative) };

        var existingTheme = app.Resources.MergedDictionaries
            .FirstOrDefault(d => d.Source != null && (d.Source.ToString().Contains("LightTheme") || d.Source.ToString().Contains("DarkTheme")));

        if (existingTheme != null)
            app.Resources.MergedDictionaries.Remove(existingTheme);

        app.Resources.MergedDictionaries.Add(dict);
    }
}