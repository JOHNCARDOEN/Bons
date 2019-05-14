using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Caliburn.Micro;
using WPF_Bestelbons.Models;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Screen = Caliburn.Micro.Screen;

namespace WPF_Bestelbons.ViewModels
{
    //public class UsersViewModel : Screen , IHandle<LoadUsersMessage> , IHandle<UserListMessage>
    public class UsersViewModel : Screen, IHandle<UserListMessage>
    {


        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;

        public int index;

        public bool CanDelete
        {
            get { return (UsersSelectedItem !=null); }
        }

        private BindableCollection<User> _usersList;

        public BindableCollection<User> UsersList
        {
            get { return _usersList; }
            set
            {
                _usersList = value;
                NotifyOfPropertyChange(() => UsersList);
            }
        }

        // public BindableCollection<User> UserList { get; set; }



        #region BINDABLE FIELDS

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                if(index >= 0)
                   UsersList[index].FirstName = _firstName;
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
                if (index >= 0)
                    UsersList[index].LastName = _lastName;
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value; 
                NotifyOfPropertyChange(() => Email);
                if (index >= 0)
                    UsersList[index].Email= _email;
            }
        }

        private string _tel;

        public string Tel
        {
            get { return _tel; }
            set
            {
                _tel = value; 
                NotifyOfPropertyChange(() => Tel);
                if (index >= 0)
                    UsersList[index].Tel = _tel;
            }
        }

        private string _handtekening;

        public string Handtekening
        {
            get { return _handtekening; }
            set
            {
                _handtekening = value;
                NotifyOfPropertyChange(() => Handtekening);
                if (index >= 0)
                    UsersList[index].Handtekening = _handtekening;
            }
        }
        #endregion

        private User _usersSelectedItem;

        public User UsersSelectedItem
        {
            get { return _usersSelectedItem; }
            set
            {
                _usersSelectedItem = value;
                //NotifyOfPropertyChange(() => UsersSelectedItem);
                if (_usersSelectedItem != null)
                {
                User userindex = UsersList.Where(user => user.LastName == _usersSelectedItem.LastName).FirstOrDefault();
                index =UsersList.IndexOf(userindex);
                LastName = _usersSelectedItem.LastName;
                FirstName = _usersSelectedItem.FirstName;
                Email = _usersSelectedItem.Email;
                Tel = _usersSelectedItem.Tel;
                Handtekening = _usersSelectedItem.Handtekening;
                }
                else
                {
                    index = -1;
                }
                NotifyOfPropertyChange(() => CanDelete);
            }
        }   
        


        public UsersViewModel(IEventAggregator eventAggregator, IWindowManager windowsmanager)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowsmanager;
            _eventAggregator.Subscribe(this);
            this.UsersList = new BindableCollection<User>();

        }



        public void Add()
        {
            User NewUser = new User();
            NewUser.FirstName = FirstName;
            NewUser.LastName = LastName;
            NewUser.Email = Email;
            NewUser.Tel = Tel;
            NewUser.Handtekening = Handtekening;
            UsersList.Add(NewUser);
            Save();
        }

        public void Clear()
        {
            UsersSelectedItem = null;
            FirstName = "";
            LastName = "";
            Email = "";
            Tel = "";
            Handtekening = "";
        }

        public void Delete()
        {
            UsersList.RemoveAt(index);
            FirstName = "";
            LastName = "";
            Email = "";
            Tel = "";
            Handtekening = "";
        }

        public void Save()
        {

            var FilePath = Properties.Settings.Default.UserListPath;
            var serializer = new XmlSerializer(typeof(BindableCollection<User>));
            using (var writer = new System.IO.StreamWriter(FilePath))
            {
                serializer.Serialize(writer,UsersList);
                writer.Flush();
            }
        }
        // After each key up event  invoking the refresh method to force te UsersList to update on the UI
        public void Change()
        {
            UsersList.Refresh();
        }

        public void Close()
        {
            TryClose(false);
        }

        public void Signature()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Signatures (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"D:\Desktop\";
            if (openFileDialog.ShowDialog() == true)
            {
                Handtekening = openFileDialog.FileName;
            }
        }

        public void Handle(UserListMessage message)
        {
            UsersList = message.userlist;
        }
    }
}
