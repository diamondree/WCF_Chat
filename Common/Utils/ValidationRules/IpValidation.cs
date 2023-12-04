using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Common.Utils.ValidationRules
{
    public class IpValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string ip = (string)value;
            var octets = ip.Split('.');
            foreach (string oct in octets)
            {
                int octet;
                int.TryParse(oct, out octet);
                if (octet < 0 || octet > 255 || oct.Length == 0 || oct.Length > 3 || octets.Count() != 4)
                    return new ValidationResult(false, "Octet must be between 0-255");
            }
            
            return new ValidationResult(true,null);
        }
    }
}
