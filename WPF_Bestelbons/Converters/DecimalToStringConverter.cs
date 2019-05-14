using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_Bestelbons.Converters
{
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            string text = value.ToString();
            decimal dec = 0.0M;
            if (decimal.TryParse(text, out dec))
                text = dec.ToString("N1", CultureInfo.InstalledUICulture);
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal dec = 0.0M;
            string stringdec = "";
            string tempstring = "";
            stringdec = value.ToString().Replace('.', ',');
            if (stringdec.Contains(','))
            {
                bool second = false;
                foreach (char c in stringdec)
                {
                    if ((c == ',' && !second) || (c !=','))
                    tempstring += c;
                    if (c == ',') second = true;
                }

                stringdec = tempstring;
            }

            decimal.TryParse(stringdec.ToString(), out dec);
            return dec;
        }
    }
}
