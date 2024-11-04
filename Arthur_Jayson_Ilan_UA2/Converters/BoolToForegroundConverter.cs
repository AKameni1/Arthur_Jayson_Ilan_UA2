using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Arthur_Jayson_Ilan_UA2.Converters
{
    internal class BoolToForegroundConverter : IValueConverter
    {
        public Brush SelectedForeground { get; set; } = Brushes.White;
        public Brush DefaultForeground { get; set; } = Brushes.White;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? SelectedForeground : DefaultForeground;
            }
            return DefaultForeground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
