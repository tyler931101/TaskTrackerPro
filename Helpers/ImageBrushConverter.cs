//using System;
//using System.Globalization;
//using System.Windows.Data;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

//namespace TicketManagementSystem.Helpers
//{
//    public class ImageBrushConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (value is BitmapImage img)
//                return new ImageBrush(img) { Stretch = Stretch.UniformToFill };

//            return new SolidColorBrush(Colors.LightGray);
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}