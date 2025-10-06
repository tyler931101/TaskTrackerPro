using System;
using System.IO;
using System.Text.Json;

namespace TicketManagementSystem.Core;

public class AppSettings
{
    public string DatabasePath { get; set; } = "ticket_system.db";
    public string Theme { get; set; } = "Light";
    public string LogLevel { get; set; } = "Info";
    public int MaxLogSizeKB { get; set; } = 1024;
}

public static class ConfigManager
{
    private static readonly string ConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
    public static AppSettings Settings { get; private set; } = new();

    static ConfigManager()
    {
        Load();
    }

    public static void Load()
    {
        try
        {
            if (File.Exists(ConfigFile))
            {
                var json = File.ReadAllText(ConfigFile);
                var config = JsonSerializer.Deserialize<AppSettings>(json);
                if (config != null)
                    Settings = config;
            }
            else
            {
                Save(); // Create default config file
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Failed to load configuration.", ex);
        }
    }

    public static void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFile, json);
        }
        catch (Exception ex)
        {
            Logger.Error("Failed to save configuration.", ex);
        }
    }
}