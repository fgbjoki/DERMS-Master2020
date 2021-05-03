using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientUI.ValidationRules.DIfferentInputValidation
{
    public class DiscreteDifferentValueRule : ValidationRule
    {
        private DifferentValueChecker differentValueChecker;

        public DifferentValueChecker DifferentValueChecker
        {
            get { return differentValueChecker; }
            set { differentValueChecker = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int inputValue;
            if (!int.TryParse(value as string, out inputValue))
            {
                return new ValidationResult(true, "");
            }

            bool different = !differentValueChecker.CurrentValue.Equals(inputValue);
            string message = different ? null : "Value must be different than the current value!";
            return new ValidationResult(different, message);
        }
    }
}
