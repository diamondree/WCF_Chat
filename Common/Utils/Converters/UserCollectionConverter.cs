using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Common.Utils.Converters
{
    public class UserCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> users = (List<string>)value;
            var sb = new StringBuilder();
            foreach (var user in users)
            {
                sb.AppendLine(user);
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
