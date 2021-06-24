using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientUI.ValidationRules
{
    public class DoubleRangeDependency : DependencyObject
    {
        public DoubleRangeDependency()
        {
            MinValue = -float.MaxValue;
            MaxValue = float.MaxValue;
        }

        public float MinValue
        {
            get { return (float)GetValue(MinValueProperty); }
            set { SetCurrentValue(MinValueProperty, value); }
        }
        public float MaxValue
        {
            get { return (float)GetValue(MaxValueProperty); }
            set { SetCurrentValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(float), typeof(DoubleRangeDependency), new PropertyMetadata((float)0));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(float), typeof(DoubleRangeDependency), new PropertyMetadata((float)0));
    }

    public class ValidationValidDepedency : DependencyObject
    {
        public bool Valid
        {
            get { return (bool)GetValue(ValidProperty); }
            set { SetCurrentValue(ValidProperty, value); }
        }

        public static readonly DependencyProperty ValidProperty = DependencyProperty.Register("Valid", typeof(bool), typeof(ValidationValidDepedency), new PropertyMetadata(false));
    }

    public class DoubleRangeValidationRule : ValidationRule
    {
        public DoubleRangeValidationRule()
        {
            DoubleRange = new DoubleRangeDependency();
            Validation = new ValidationValidDepedency();
        }

        public DoubleRangeDependency DoubleRange { get; set; }

        public ValidationValidDepedency Validation { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            float doubleValue = 0;

            if (!(value is string))
            {
                Validation.Valid = false;
                return new ValidationResult(false, "Provided value is not a number.");
            }

            if (!float.TryParse(((string)value), out doubleValue))
            {
                Validation.Valid = false;
                return new ValidationResult(false, "Provided value is not a number.");
            }

            if ((doubleValue < DoubleRange.MinValue) || (doubleValue > DoubleRange.MaxValue))
            {
                Validation.Valid = false;
                return new ValidationResult(false,
                  $"Please enter a value in the range: ({DoubleRange.MinValue})  -  ({DoubleRange.MaxValue}).");
            }

            Validation.Valid = true;
            return ValidationResult.ValidResult;
        }
    }
}
