using System.Globalization;
using System.Windows.Controls;

namespace FieldSimulator.Validations
{
    public class DiscreteValuesValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            short shortValue;
            bool isValid;
            if (!short.TryParse(value as string, out shortValue))
            {
                isValid = false;
            }
            else
            {
                isValid = shortValue == 0 || shortValue == 1;
            }

            ValidationResult validation = new ValidationResult(isValid, "Discrete values can must be in range [0 - 1]");

            return validation;
        }
    }
}
