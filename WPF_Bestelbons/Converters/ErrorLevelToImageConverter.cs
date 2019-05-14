using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_Bestelbons.Models;
using System.Windows.Controls;

namespace WPF_Bestelbons.Converters
{
    public class ErrorLevelToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage image;
            image = new BitmapImage(new Uri("pack://application:,,,/WPF_Bestelbons;component/Views/Resources/INFO.png"));
            switch ((ErrorLevel)value)
            {
                case ErrorLevel.Error:
                   image = new BitmapImage(new Uri("pack://application:,,,/WPF_Bestelbons;component/Views/Resources/ERROR.png"));
                    return image;

                case ErrorLevel.Warning:
                    image = new BitmapImage(new Uri("pack://application:,,,/WPF_Bestelbons;component/Views/Resources/WARNING.png"));
                    return image;
                case ErrorLevel.Info:
                    image = new BitmapImage(new Uri("pack://application:,,,/WPF_Bestelbons;component/Views/Resources/INFO.png"));
                    return image;

                default: return image;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
