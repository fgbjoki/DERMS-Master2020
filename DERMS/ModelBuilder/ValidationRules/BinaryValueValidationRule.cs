using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientUI.ValidationRules
{
    public class BinaryValueValidationRule : IsTextNumberValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult baseResult = base.Validate(value, cultureInfo);

            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            double number = double.Parse(value as string);

            if (number < 0 || number > 1)
            {
                return new ValidationResult(false, "Not a binary value!");
            }

            return new ValidationResult(true, null);
        }
    }
}
