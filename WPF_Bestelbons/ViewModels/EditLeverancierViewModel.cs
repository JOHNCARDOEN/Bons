using Caliburn.Micro;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons.ViewModels
{
    public class EditLeverancierViewModel : Screen, IHandle<LeveranciersChange>
    {
        private readonly IEventAggregator _eventAggregator;

        private bool _emailValid;

        public bool EmailValid
        {
            get { return _emailValid; }
            set
            {
                _emailValid = value;
                NotifyOfPropertyChange(() => EmailValid);
            }
        }


        private bool _editSaved;

        public bool EditSaved
        {
            get { return _editSaved; }
            set
            {
                _editSaved = value;
                NotifyOfPropertyChange(() => EditSaved);
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

        private Leverancier _editedLeverancier;

        public bool CanEditLeverancier
        {
            get { return !string.IsNullOrEmpty(EditedLeverancier.Name) &&(EmailValid || string.IsNullOrEmpty(EditedLeverancier.Email)); }
        }

        public Leverancier EditedLeverancier
        {
            get { return _editedLeverancier; }
            set
            {
                _editedLeverancier = value;
                EmailValid = _editedLeverancier.EmailValid;
                NotifyOfPropertyChange(() => EditedLeverancier);
                NotifyOfPropertyChange(() => CanEditLeverancier);
            }
        }

        public EditLeverancierViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            EditedLeverancier = new Leverancier();
            EditSaved = false;
        }

        public void EditLeverancier()
        {

            _eventAggregator.PublishOnUIThread(message: new AddLeverancier(EditedLeverancier, true));
            EditSaved = true;
        }

        public void OK()
        {
            TryClose();
        }

        public void CloseButton()
        {
            TryClose(false);
        }

        public void Handle(LeveranciersChange message)
        {
            EditedLeverancier = message.Leverancier;
        }

        public void KeyUp()
        {
            EditSaved = false;
            EmailValid = _editedLeverancier.EmailValid;
            NotifyOfPropertyChange(() => CanEditLeverancier);
        }

    }
}
