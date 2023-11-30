using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Common
{
    public class MessageCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<string> messages = (ObservableCollection<string>)value;

            var sb = new StringBuilder();

            foreach (var message in messages)
            {
                sb.AppendLine(message);
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
