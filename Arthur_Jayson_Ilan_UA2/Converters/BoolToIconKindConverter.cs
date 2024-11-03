using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MahApps.Metro.IconPacks;

namespace Arthur_Jayson_Ilan_UA2.Converters
{
    public class BoolToIconKindConverter : IValueConverter
    {
        public PackIconMaterialKind TrueIcon {  get; set; } = PackIconMaterialKind.Eye;
        public PackIconMaterialKind FalseIcon {  get; set; } = PackIconMaterialKind.EyeOff;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPasswordVisible = (bool)value;
            return isPasswordVisible ? TrueIcon : FalseIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(TrueIcon);
        }
    }
}
