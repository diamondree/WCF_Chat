using System.Globalization;
using System.Windows.Controls;

namespace Common.Utils.ValidationRules
{
    public class PortValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int port;
            int.TryParse((string)value, out port);
            if (port < 1 || port > 65535)
                return new ValidationResult(false, "Port must be between 1 and 65535");
            return new ValidationResult(true, null);
        }
    }
}
