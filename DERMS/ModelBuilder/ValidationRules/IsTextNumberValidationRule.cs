using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientUI.ValidationRules
{
    public class IsTextNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string stringValue = value as string;

            if (stringValue == null)
            {
                return new ValidationResult(false, "Input field is not a number");
            }

            double number;

            if (!double.TryParse(stringValue, out number))
            {
                return new ValidationResult(false, "Input field is not a number");
            }

            return new ValidationResult(true, null);
        }
    }
}
