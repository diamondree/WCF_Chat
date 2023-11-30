using System.Globalization;
using System.Windows.Controls;

namespace Common.Utils.ValidationRules
{
    public class IpOctetValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int octet;
            int.TryParse((string)value, out octet);
            if (octet < 0 || octet > 255)
                return new ValidationResult(false, "Octet must be between 0-255");
            return new ValidationResult(true,null);
        }
    }
}
