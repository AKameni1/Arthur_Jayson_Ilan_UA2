using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Arthur_Jayson_Ilan_UA2.Converters
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public bool CollapsedWhenHidden { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value as bool? ?? false;
            if (!boolValue)
                return Visibility.Visible;
            else 
                return CollapsedWhenHidden ? Visibility.Collapsed: Visibility.Hidden; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return visibility != Visibility.Visible;

            return true;
        }
    }
}
