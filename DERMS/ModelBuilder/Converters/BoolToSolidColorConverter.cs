﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientUI.Converters
{
    public class BoolToSolidColorConverter : IValueConverter
    {
        private static SolidColorBrush green = new SolidColorBrush(Colors.Green);
        private static SolidColorBrush blue = new SolidColorBrush(Colors.Blue);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool conduction = (bool)value;

            return conduction ? green : blue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
