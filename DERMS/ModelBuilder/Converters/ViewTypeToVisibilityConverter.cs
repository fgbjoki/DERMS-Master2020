using ClientUI.Common.ViewType;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ClientUI.Converters
{
    public class ViewTypeToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Checks if the selected view type <paramref name="parameter"/> is the same as objects <see cref="ViewTypeEnum"/> .
        /// </summary>
        /// <param name="value">UIElements view type.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Selected view type.</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ViewTypeEnum uiElementsViewType = (ViewTypeEnum)value;
            ViewTypeEnum selectedViewType = (ViewTypeEnum)parameter;

            return uiElementsViewType == selectedViewType ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
