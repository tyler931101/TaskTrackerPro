//using System;
//using System.Windows;
//using System.Windows.Media;

//namespace TicketManagementSystem.Helpers
//{
//    public static class CircleClipHelper
//    {
//        public static readonly DependencyProperty EnableCircleClipProperty =
//            DependencyProperty.RegisterAttached(
//                "EnableCircleClip",
//                typeof(bool),
//                typeof(CircleClipHelper),
//                new PropertyMetadata(false, OnEnableCircleClipChanged));

//        public static bool GetEnableCircleClip(UIElement element) =>
//            (bool)element.GetValue(EnableCircleClipProperty);

//        public static void SetEnableCircleClip(UIElement element, bool value) =>
//            element.SetValue(EnableCircleClipProperty, value);

//        private static void OnEnableCircleClipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            if (d is FrameworkElement fe && (bool)e.NewValue)
//            {
//                fe.SizeChanged += (s, _) =>
//                {
//                    double radius = Math.Min(fe.ActualWidth, fe.ActualHeight) / 2;
//                    fe.Clip = new EllipseGeometry
//                    {
//                        Center = new Point(fe.ActualWidth / 2, fe.ActualHeight / 2),
//                        RadiusX = radius,
//                        RadiusY = radius
//                    };
//                };
//            }
//        }
//    }
//}