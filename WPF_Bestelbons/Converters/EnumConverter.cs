using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static WPF_Bestelbons.Models.Bestelbonregel;

namespace WPF_Bestelbons.Converters
{
    public class EnumConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is EA eA)
            {
                return GetString(eA);
            }
            else
                return "unknown";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return value;
            }
            return null;
        }

        public string[] Strings => GetStrings();

        public static string GetString(EA eA)
        {
            return eA.ToString();
        }


        public static string[] GetStrings()
        {
            List<string> list = new List<string>();
            foreach (EA eA in Enum.GetValues(typeof(EA)))
            {
                list.Add(GetString(eA));
            }

            return list.ToArray();
        }
    }
}
