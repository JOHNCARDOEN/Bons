using Caliburn.Micro;
using System.IO;
using System.Linq;

namespace WPF_Bestelbons.ViewModels
{
    public class BestelbonsViewModel : Screen, IHandle<BestelbonAdded>, IHandle<BestelbonsPathChanged>
    {
        private readonly IEventAggregator _eventAggregator;

        private BindableCollection<string> _bestelbonsLijst;

        public BindableCollection<string> BestelbonsLijst
        {
            get { return _bestelbonsLijst; }
            set
            {
                _bestelbonsLijst = value;
                NotifyOfPropertyChange(() => BestelbonsLijst);
            }
        }

        private BindableCollection<string> _bestelbonsLijstUI;

        public BindableCollection<string> BestelbonsLijstUI
        {
            get { return _bestelbonsLijstUI; }
            set
            {
                _bestelbonsLijstUI = value;
                NotifyOfPropertyChange(() => BestelbonsLijstUI);
            }
        }

        private string _selection;

        public string Selection
        {
            get { return _selection; }
            set
            {
                _selection = value;
                NotifyOfPropertyChange(() => Selection);
            }
        }

        private string _bestelbonsLijstSelectedItem;

        public string BestelbonsLijstSelectedItem
        {
            get { return _bestelbonsLijstSelectedItem; }
            set
            {
                _bestelbonsLijstSelectedItem = value;
                NotifyOfPropertyChange(() => BestelbonsLijstSelectedItem);
                if (BestelbonsLijstSelectedItem != null)
                {
                    _eventAggregator.PublishOnUIThread(message: new BestelbonChange(BestelbonsLijstSelectedItem));
                }
            }
        }


        public BestelbonsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            BestelbonsLijst = new BindableCollection<string>();
            LoadBestelbons();
        }


        public void LoadBestelbons()
        {
            string path = Properties.Settings.Default.BestelbonsPath;
            if (!string.IsNullOrEmpty(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                BestelbonsLijst.Clear();
                foreach (var file in directoryInfo.GetFiles())
                {
                    if (file.Extension.Contains("abb"))
                    {
                        BestelbonsLijst.Add(file.Name);

                    }


                }

            }

            BestelbonsLijstUI = BestelbonsLijst;
        }

        public void Select()
        {
            string SelectionLower = Selection.ToLower();
            BindableCollection<string> ListWithSelection = new BindableCollection<string>();
            string[] substrings = SelectionLower.Split('+');

            for (int i = 0; i < BestelbonsLijst.Count; i++)
            {
                BestelbonsLijst[i] = BestelbonsLijst[i].ToLower();
            }
            foreach (var bestelbon in BestelbonsLijst)
            {
                if (substrings.All(bestelbon.Contains))
                    ListWithSelection.Add(bestelbon);
            }

            //{
            //    if (bestelbon.ToLower().Contains(Selection.ToLower())) ListWithSelection.Add(bestelbon);
            //}
            BestelbonsLijstUI = ListWithSelection;
            if (string.IsNullOrEmpty(Selection)) BestelbonsLijstUI = BestelbonsLijst;

        }

        public void Close()
        {
            _eventAggregator.PublishOnUIThread(message: new CloseMessage());
        }

        public void Handle(BestelbonAdded message)
        {
            LoadBestelbons();
        }

        public void Handle(BestelbonsPathChanged message)
        {
            LoadBestelbons();
        }
    }
}
