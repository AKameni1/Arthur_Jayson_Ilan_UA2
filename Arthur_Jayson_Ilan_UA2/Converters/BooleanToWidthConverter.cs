using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arthur_Jayson_Ilan_UA2.Converters
{
    public class BooleanToWidthConverter : IValueConverter
    {
        public double ExpandedWidth { get; set; } = 200;
        public double CollapsedWidth { get; set; } = 85;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isExpanded)
            {
                return isExpanded ? ExpandedWidth : CollapsedWidth;
            }
            return ExpandedWidth; // Valeur par défaut
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
