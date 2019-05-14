using System.Collections.Generic;
using Caliburn.Micro;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace WPF_Bestelbons.Models
{
    public class DataServiceLeveranciers : IDataServiceLeveranciers
    {

        public DataServiceLeveranciers()
        {

        }
        public BindableCollection<Leverancier> GetAllLeveranciers(BindableCollection<Leverancier> Leverancierlist)
        {
            /// This User Settings defined in Properties.Settings.Default.LeveranciersListPath are stored in XAML format in the directory :
            /// C:[Users]\[User]\AppData\Local\[ApplicationName]\[ApplicationName.exe_Url_XXXXXXXXXXXXX]\1.0.0.0

            string FilePath = Properties.Settings.Default.LeveranciersListPath;
            var serializer = new XmlSerializer(typeof(BindableCollection<Leverancier>));
            using (var stream = File.OpenRead(FilePath))
            {
                var other = (BindableCollection<Leverancier>)(serializer.Deserialize(stream));
                Leverancierlist.Clear();
                Leverancierlist.AddRange(other);
            }
            return Leverancierlist;
        }
    }
}
