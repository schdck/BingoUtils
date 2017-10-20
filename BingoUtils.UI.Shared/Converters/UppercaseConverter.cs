using System;
using System.Globalization;
using System.Windows.Data;

namespace BingoUtils.UI.Shared.Converters
{
    public class UppercaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
