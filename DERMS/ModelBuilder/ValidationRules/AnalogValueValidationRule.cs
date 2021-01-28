using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientUI.ValidationRules
{
    class AnalogValueValidationRule : IsTextNumberValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult baseResult = base.Validate(value, cultureInfo);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            double number = double.Parse(value as string);

            if (number > float.MaxValue - 1 || number < float.MinValue + 1)
            {
                return new ValidationResult(false, "Value exceeded range for given remote point.");
            }

            return new ValidationResult(true, null);
        }
    }
}
