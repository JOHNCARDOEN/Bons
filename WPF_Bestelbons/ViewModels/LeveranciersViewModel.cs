using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using WPF_Bestelbons.Models;
using WPF_Bestelbons.Properties;


namespace WPF_Bestelbons.ViewModels
{

    public class LeveranciersViewModel : Screen, IHandle<AddLeverancier> , IHandle<LoadLeveranciersMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly IDataServiceLeveranciers _dataserviceLeveranciers;
        private Leverancier _leveranciersSeletedItem;

        public EditLeverancierViewModel EditLeveranciersVM { get; set; }

        BindableCollection<Leverancier> ListWithSelection = new BindableCollection<Leverancier>();

        public bool CanDelete
        {
            get { return LeveranciersSelectedItem != null; }
        }

        public bool CanAdd
        {
            get { return true; } //!AddLeverancierVM.IsActive; }
        }

        public bool CanEdit
        {
            get { return LeveranciersSelectedItem != null; }
        }

        public Leverancier LeveranciersSelectedItem
        {
            get { return _leveranciersSeletedItem; }
            set
            {
                _leveranciersSeletedItem = value;
                NotifyOfPropertyChange(() => LeveranciersSelectedItem);
                NotifyOfPropertyChange(() => CanDelete);
                NotifyOfPropertyChange(() => CanEdit);
                if (LeveranciersSelectedItem != null)
                {
                    _eventAggregator.PublishOnUIThread(message: new LeveranciersChange(LeveranciersSelectedItem));
                }

            }
        }



        private BindableCollection<Leverancier> _leveranciersLijst;

        public BindableCollection<Leverancier> LeveranciersLijst
        {
            get
            {
                return _leveranciersLijst;
            }
            set
            {
                _leveranciersLijst = value;
                NotifyOfPropertyChange(() => LeveranciersLijst);
            }

        }

        private BindableCollection<Leverancier> _leveranciersLijstUI;

        public BindableCollection<Leverancier> LeveranciersLijstUI
        {
            get { return _leveranciersLijstUI; }
            set
            {
                _leveranciersLijstUI = value;
                NotifyOfPropertyChange(() => LeveranciersLijstUI);
            }
        }

        private string _search;

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value; 
                NotifyOfPropertyChange(() => Search);
            }
        }

        public LeveranciersViewModel(IEventAggregator eventAggregator,IWindowManager windowsmanager, IDataServiceLeveranciers dataserviceLeveranciers)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowsmanager;
            _dataserviceLeveranciers = dataserviceLeveranciers;
            _eventAggregator.Subscribe(this);
            EditLeveranciersVM = IoC.Get<EditLeverancierViewModel>();                                                                                   
            this.LeveranciersLijst = new BindableCollection<Leverancier>();
            this.LeveranciersLijstUI = new BindableCollection<Leverancier>();

        }

        public void Add()
        {
            var addLeverancierViewModel = IoC.Get<AddLeverancierViewModel>();
            addLeverancierViewModel.Capiton = "Add Leverancier";
            var result = _windowManager.ShowDialog(addLeverancierViewModel);
        }

        public void Save()
        {
            var FilePath = Properties.Settings.Default.LeveranciersListPath;
            var serializer = new XmlSerializer(typeof(BindableCollection<Leverancier>));
            using (var writer = new System.IO.StreamWriter(FilePath))
            {
                serializer.Serialize(writer, LeveranciersLijst);
                writer.Flush();
                _eventAggregator.PublishOnUIThread(new LeveranciersListUpdated(LeveranciersLijst));
            }

        }

        public void Select()
        {
            ListWithSelection.Clear();
            foreach (var leverancier in LeveranciersLijst)
            {
                if (leverancier.Name.ToLower().Contains(Search.ToLower()))
              {
                    ListWithSelection.Add(leverancier);
                }


            }
            LeveranciersLijstUI = ListWithSelection;
            if (string.IsNullOrEmpty(Search)) LeveranciersLijstUI = LeveranciersLijst;

        }

        public void Delete()
        {
            LeveranciersLijst.Remove(LeveranciersSelectedItem);
            _eventAggregator.PublishOnUIThread(message: new LeveranciersChange(new Leverancier() {Name=""}));
        }


        public void Edit()
        {
            EditLeveranciersVM.Capiton = "Edit Leverancier";
            var result = _windowManager.ShowDialog(EditLeveranciersVM);
        }


        public void Handle(AddLeverancier message)
        {
            if (!message.Edit)
            {
                Leverancier leverancier = new Leverancier();
                leverancier = message.Leverancier;
                LeveranciersLijst.Add(leverancier); 
            }
            
            // OrderBy does NOT change the original Collection !! Original Collection must be recreated !!!
            LeveranciersLijst= new BindableCollection<Leverancier>(LeveranciersLijst.OrderBy(i => i.Name));
            Save();
            LeveranciersLijstUI = LeveranciersLijst;

        }

        public void Close()
        {
            _eventAggregator.PublishOnUIThread(message: new CloseMessage());
        }

        public void Handle(LoadLeveranciersMessage message)
        {
            if (File.Exists(Properties.Settings.Default.LeveranciersListPath))
            {
                LeveranciersLijst = _dataserviceLeveranciers.GetAllLeveranciers(LeveranciersLijstUI);
                _eventAggregator.PublishOnUIThread(new LeveranciersListUpdated(LeveranciersLijst));
            }
            else
            {
                var dialogViewModel = IoC.Get<DialogViewModel>();
                dialogViewModel.Capiton = "File Open";
                dialogViewModel.Message = "LeveranciersDirectory not yet set or file removed !";
                var result = _windowManager.ShowDialog(dialogViewModel);
            }
        }
    }
}
