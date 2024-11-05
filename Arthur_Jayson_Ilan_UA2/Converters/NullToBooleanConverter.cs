using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arthur_Jayson_Ilan_UA2.Converters
{
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNotNull = value != null;

            // Inversion de la logique si le paramètre "invert" est passé
            if (parameter != null && parameter?.ToString()?.ToLower() == "invert")
            {
                isNotNull = !isNotNull;
            }

            return isNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
