using System;
using System.IO;

namespace TicketManagementSystem.Core;

public static class Logger
{
    private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");
    private static readonly long MaxFileSize = 1 * 1024 * 1024; // 1MB

    private static readonly object LockObj = new();

    public static void Info(string message) => Write("INFO", message);
    public static void Warning(string message) => Write("WARNING", message);
    public static void Error(string message, Exception? ex = null) => Write("ERROR", $"{message} {ex?.Message}");

    private static void Write(string level, string message)
    {
        try
        {
            lock (LockObj)
            {
                RotateIfTooLarge();

                var logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                File.AppendAllText(LogFilePath, logLine + Environment.NewLine);
            }
        }
        catch
        {
            // avoid logging failures crashing app
        }
    }

    private static void RotateIfTooLarge()
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                var fileInfo = new FileInfo(LogFilePath);
                if (fileInfo.Length > MaxFileSize)
                {
                    var backup = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"app_{DateTime.Now:yyyyMMddHHmmss}.log");
                    File.Move(LogFilePath, backup);
                }
            }
        }
        catch
        {
            // ignore rotation errors
        }
    }
}


//Logger.Info("Application started.");
//Logger.Warning("Login attempt failed.");
//Logger.Error("Database connection failed.", ex);