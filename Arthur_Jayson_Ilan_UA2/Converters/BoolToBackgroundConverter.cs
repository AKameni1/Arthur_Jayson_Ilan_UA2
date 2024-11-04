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
    internal class BoolToBackgroundConverter : IValueConverter
    {
        public Brush SelectedBackground { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B5CD6"));
        public Brush DefaultBackground { get; set; } = new SolidColorBrush(Colors.Transparent);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? SelectedBackground : DefaultBackground;
            }
            return DefaultBackground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
