using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace WPF_Bestelbons.Converters
{
    public class BoolToRectangleFill :IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((bool)value == false)
            {
                SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007FFF"));//System.Windows.Media.Brushes.Orange;
                return brush;
            }
            else
            {
                SolidColorBrush brush = System.Windows.Media.Brushes.Green;
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
