using System;
using System.Windows;
using TicketManagementSystem.Core;

namespace TicketManagementSystem;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ConfigManager.Load();
        Logger.Info("Application started.");

        if (ConfigManager.Settings.Theme == "Dark")
        {
            ThemeManager.ToggleTheme();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Logger.Info("Application closed.");
        base.OnExit(e);
    }
}