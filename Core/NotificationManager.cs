using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TicketManagementSystem.Core;

public static class NotificationManager
{
    private static readonly TimeSpan DisplayDuration = TimeSpan.FromSeconds(3);

    public static void Show(string message, string type = "Info")
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var window = Application.Current.MainWindow;
            if (window == null) return;

            // Create container if missing
            if (window.FindName("NotificationContainer") is not StackPanel container)
            {
                container = new StackPanel
                {
                    Name = "NotificationContainer",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 20, 20, 0),
                };
                Panel.SetZIndex(container, 9999);
                (window.Content as Grid)?.Children.Add(container);
                window.RegisterName(container.Name, container);
            }

            // Toast card
            Border toast = new()
            {
                Background = GetBackground(type),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(15),
                Opacity = 0,
                Width = 300,
                Child = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.White,
                    FontSize = 15,
                    TextWrapping = TextWrapping.Wrap
                }
            };

            container.Children.Insert(0, toast);

            // Fade in
            var fadeIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(200)));
            toast.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            // Auto remove after 3 seconds
            var timer = new System.Timers.Timer(DisplayDuration.TotalMilliseconds);
            timer.Elapsed += (s, e) =>
            {
                timer.Stop();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(300)));
                    fadeOut.Completed += (s2, e2) => container.Children.Remove(toast);
                    toast.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                });
            };
            timer.Start();
        });
    }

    private static Brush GetBackground(string type)
    {
        return type switch
        {
            "Success" => new SolidColorBrush(Color.FromRgb(34, 197, 94)),   // green
            "Error" => new SolidColorBrush(Color.FromRgb(239, 68, 68)),     // red
            "Warning" => new SolidColorBrush(Color.FromRgb(234, 179, 8)),   // yellow
            _ => new SolidColorBrush(Color.FromRgb(59, 130, 246)),          // blue
        };
    }
}