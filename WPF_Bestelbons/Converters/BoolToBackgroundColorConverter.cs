using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_Bestelbons.Converters
{
     public class BoolToBackgroundColorConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((bool)value == true)
            {
                SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CC4E06"));//System.Windows.Media.Brushes.Orange;
                return brush;
            }
            else
            {
                SolidColorBrush brush = System.Windows.Media.Brushes.Transparent;
                return brush;
            }


            throw new ArgumentException("Value is not valid");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
