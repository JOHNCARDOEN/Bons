using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons.ViewModels
{
    public class AddLeverancierViewModel : Screen

    {
        private readonly IEventAggregator _eventAggregator;


        private bool _addedSaved;

        public bool AddedSaved
        {
            get { return _addedSaved; }
            set
            {
                _addedSaved = value;
                NotifyOfPropertyChange(() => AddedSaved);
            }
        }       

        private string _capiton;

        public string Capiton
        {
            get { return _capiton; }
            set
            {
                _capiton = value;
                NotifyOfPropertyChange(() => Capiton);
            }
        }

        private Leverancier _addedLeverancier;

        public bool CanAddLeverancier
        {
            get { return (!String.IsNullOrEmpty(AddedLeverancier.Name)); }
        }

        public Leverancier AddedLeverancier
        {
            get { return _addedLeverancier; }
            set { _addedLeverancier = value;
                NotifyOfPropertyChange(() => AddedLeverancier);
 //               NotifyOfPropertyChange(() => CanAddLeverancier);
            }
        }

        public AddLeverancierViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            this.AddedLeverancier = new Leverancier();
            AddedSaved = false;
        }

        public void AddLeverancier()
        {
            _eventAggregator.PublishOnUIThread(message: new AddLeverancier(AddedLeverancier, false));
            AddedSaved = true;
            AddedLeverancier = new Leverancier();
        }

        public void Clear()
        {
            AddedLeverancier.Name = "";
            AddedLeverancier.City = "";
            AddedLeverancier.Email = "";
            AddedLeverancier.Number = "";
            AddedLeverancier.Postcode = "";
            AddedLeverancier.Street = "";
            AddedLeverancier.Tel = "";
            NotifyOfPropertyChange(() => AddedLeverancier);
        }

        public void OK()
        {
            TryClose();
        }

        public void CloseButton()
        {
            TryClose(false);
        }

        public void KeyUp()
        {
            NotifyOfPropertyChange(() => AddedLeverancier);
            NotifyOfPropertyChange(() => CanAddLeverancier);
            AddedSaved = false;
        }

    }
}
