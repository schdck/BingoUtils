using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BingoUtils.UI.QuestionsSorter.ValueConverters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private object GetVisibility(object value)
        {
            if (!(value is bool))
            {
                return Visibility.Collapsed;
            }
                
            bool objValue = (bool)value;

            if (objValue)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVisibility(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
