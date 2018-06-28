using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace FirstUwpApp.ViewModel
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object colorValue, Type targetType, object parameter, string language)
        {
            return new SolidColorBrush((Color)colorValue);
        }

        public object ConvertBack(object brushValue, Type targetType, object parameter, string language)
        {
            return ((SolidColorBrush)brushValue).Color;
        }
    }
}
