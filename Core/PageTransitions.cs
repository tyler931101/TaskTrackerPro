using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TicketManagementSystem.Core;

public static class PageTransitions
{
    public static void FadeIn(Page page)
    {
        var fade = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(400)));
        page.BeginAnimation(UIElement.OpacityProperty, fade);
    }
}