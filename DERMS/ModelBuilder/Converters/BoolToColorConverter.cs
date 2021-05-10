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
    public class BoolToColorConverter : IValueConverter
    {
        private static SolidColorBrush green = Brushes.Green;
        private static SolidColorBrush red = Brushes.Red;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool conduction = (bool)value;

            return conduction ? green : red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
