using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons.ViewModels
{
    public class SelectUserViewModel : Screen , IHandle<UserListMessage>
    {
        private readonly IEventAggregator _eventAggregator;

        public BindableCollection<User> UserList { get; set; }

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

        private User __selectedItem;

        public User SelectedItem
        {
            get { return __selectedItem; }
            set
            {
                __selectedItem = value; 
                NotifyOfPropertyChange(() => SelectedItem);
                _eventAggregator.PublishOnUIThread(message: new UserChangedMessage(SelectedItem));
            }
        }

        public SelectUserViewModel(IEventAggregator eventaggregator)
        {
            this.UserList = new BindableCollection<User>();
            _eventAggregator = eventaggregator;
            _eventAggregator.Subscribe(this);
        }

        public void OK()
        {
            TryClose();
        }

        public void CloseButton()
        {
            TryClose(false);
        }

        public void Handle (UserListMessage message)
        {
            UserList = message.userlist;
        }
    }
}
