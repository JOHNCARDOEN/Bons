using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace WPF_Bestelbons.ViewModels
{
    public class LeveringsvoorwaardenViewModel : Screen , IHandle<LoadLeveringsvoorwaardenMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private string _leveringsvoorwaarden;

        public string Leveringsvoorwaarden
        {
            get { return _leveringsvoorwaarden; }
            set
            {
                _leveringsvoorwaarden = value; 
                NotifyOfPropertyChange(() => Leveringsvoorwaarden);
            }
        }

        public LeveringsvoorwaardenViewModel(IEventAggregator eventAggregator, IWindowManager windowsmanager)
        {
            _windowManager = windowsmanager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public void Save()
        {


            var FilePath = Properties.Settings.Default.Leveringsvw;
            var serializer = new XmlSerializer(typeof(String));
            using (var writer = new System.IO.StreamWriter(FilePath))
            {
                serializer.Serialize(writer, Leveringsvoorwaarden);
                writer.Flush();
                Close();
            }
        }

        public void Close()
        {
            TryClose(false);
        }

        public void Handle(LoadLeveringsvoorwaardenMessage message)
        {
            if (File.Exists(Properties.Settings.Default.LeveranciersListPath))
            {
                string FilePathLevvw = Properties.Settings.Default.Leveringsvw;
                var serializerlevvw = new XmlSerializer(typeof(String));
                using (var stream = File.OpenRead(FilePathLevvw))
                {
                    var other = (String)(serializerlevvw.Deserialize(stream));
                    Leveringsvoorwaarden = "";
                    Leveringsvoorwaarden = other;
                }

            }
            else
            {
                var dialogViewModel = IoC.Get<DialogViewModel>();
                dialogViewModel.Capiton = "File Open";
                dialogViewModel.Message = "Leveringsvoorwaarden not yet set or file removed !";
                var result = _windowManager.ShowDialog(dialogViewModel);
            }
        }
    }
}
