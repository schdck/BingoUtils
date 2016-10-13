using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BingUtils.UI.QuestionsSorter.ValueConverters
{
    public class PercentageToBrushConverter : IValueConverter
    {
        public static double MaxSemelhanca;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = double.Parse(value.ToString());
            double opacity = 0.3;
            string color;

            if(d < (MaxSemelhanca / 5))
            {
                color = "#00FF00";
                opacity = 0.5;
            }
            else if(d >= (MaxSemelhanca / 5) && d < (MaxSemelhanca / 5) * 2)
            {
                color = "#00CD00";
            }
            else if(d >= (MaxSemelhanca / 5) * 2 && d < (MaxSemelhanca / 5) * 3)
            {
                color = "#008B00";
            }
            else if(d >= (MaxSemelhanca / 5) * 3 && d < (MaxSemelhanca / 5) * 4)
            {
                color = "#FA8072";
            }
            else
            {
                color = "#FF0000";

                if(d >= MaxSemelhanca)
                {
                    opacity = 0.5;
                }
            }
            var brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(color));

            brush.Opacity = opacity;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush b = value as SolidColorBrush;

            return 0;
        }
    }
}
