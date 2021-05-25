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
    class LoadToLinerGradientConverter : IMultiValueConverter
    {
        private static readonly LinearGradientBrush red = CreateRedGradientBrush();
        private static readonly LinearGradientBrush orange = CreateOrangeGradientBrush();
        private static readonly LinearGradientBrush green = CreateGreenGradientBrush();
        private static readonly LinearGradientBrush yellow = CreateYellowGradientBrush();
        private static readonly SolidColorBrush black = new SolidColorBrush(Colors.Black);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            float currentValue = (float)values[1];
            float maximalValue = (float)values[2];

            if (currentValue <= 0)
            {
                return black;
            }

            float percentage = currentValue / maximalValue * 100;

            if (percentage >= 80)
            {
                return red;
            }

            if (percentage >= 50)
            {
                return orange;
            }

            if (percentage >= 30)
            {
                return yellow;
            }

            return green;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
