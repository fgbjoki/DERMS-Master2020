using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientUI.Converters
{
    public class StateOfChargeToLinearGradientConverter : IValueConverter
    {
        private static readonly LinearGradientBrush red = CreateRedGradientBrush();
        private static readonly LinearGradientBrush orange = CreateOrangeGradientBrush();
        private static readonly LinearGradientBrush green = CreateGreenGradientBrush();
        private static readonly LinearGradientBrush yellow = CreateYellowGradientBrush();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is float))
            {
                return Colors.Transparent;
            }

            float floatValue = (float)value;

            if (floatValue <= 30)
            {
                return red;
            }

            if (floatValue <= 50)
            {
                return orange;
            }

            if (floatValue <= 75)
            {
                return yellow;
            }

            return green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static LinearGradientBrush CreateRedGradientBrush()
        {
            var gradientColors = new GradientStopCollection();
            gradientColors.Add(new GradientStop(Color.FromRgb(255, 13, 13), 0));
            gradientColors.Add(new GradientStop(Color.FromRgb(255, 142, 21), 1));
            return new LinearGradientBrush(gradientColors);
        }

        private static LinearGradientBrush CreateOrangeGradientBrush()
        {
            var gradientColors = new GradientStopCollection();
            gradientColors.Add(new GradientStop(Colors.OrangeRed, 0));
            gradientColors.Add(new GradientStop(Colors.Orange, 1));
            return new LinearGradientBrush(gradientColors);
        }

        private static LinearGradientBrush CreateYellowGradientBrush()
        {
            var gradientColors = new GradientStopCollection();
            gradientColors.Add(new GradientStop(Colors.Orange, 0));
            gradientColors.Add(new GradientStop(Color.FromRgb(250, 241, 51), 1));
            return new LinearGradientBrush(gradientColors);
        }

        private static LinearGradientBrush CreateGreenGradientBrush()
        {
            var gradientColors = new GradientStopCollection();
            gradientColors.Add(new GradientStop(Colors.Green, 0));
            gradientColors.Add(new GradientStop(Colors.LimeGreen, 1));
            return new LinearGradientBrush(gradientColors);
        }
    }
}
