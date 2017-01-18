using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BingoUtils.UI.Shared.Converters
{
    public class SliderForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)value;

            Brush foreground;

            if(progress <= 10)
            {
                foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0000"));
            }
            else if(progress <= 20)
            {
                foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF4500"));
            }
            else if (progress <= 30)
            {
                foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7256"));
            }
            else if(progress <= 45)
            {
                foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F00"));
            }
            else if (progress <= 60)
            {
                foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
            }
            else if(progress <= 70)
            {
                foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#6B8E23"));
            }
            else
            {
                foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#80BA45"));
            }

            return foreground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
