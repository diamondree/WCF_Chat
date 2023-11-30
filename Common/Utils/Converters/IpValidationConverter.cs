using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Utils.Converters
{
    public class IpValidationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length > 0)
            {
                foreach (var value in values)
                {
                    if (value is bool && (bool)value)
                        return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
