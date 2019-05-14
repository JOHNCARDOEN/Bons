using Caliburn.Micro;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;
using WPF_Bestelbons.Models;


namespace WPF_Bestelbons.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LeveranciersChange>, IHandle<UserChangedMessage>, IHandle<CloseMessage>, IHandle<ErrorMessage>
    {
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private WindowState _windowState;
        double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        private string _buttonImage;
        private double _maxHeight;
        private Bestelbon bestelbons;
        private string _version;

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                NotifyOfPropertyChange(() => Version);
            }
        }

        public Bestelbon Bestelbons
        {
            get { return bestelbons; }
            set { bestelbons = value; }
        }

        #region BINDABLE FIELDS

        private BindableCollection<Error> _errorList;

        public BindableCollection<Error> ErrorList
        {
            get { return _errorList; }
            set
            {
                _errorList = value;
                NotifyOfPropertyChange(() => ErrorList);
            }
        }

        private User _currentuser;

        public User CurrentUser
        {
            get { return _currentuser; }
            set
            {
                _currentuser = value;
                NotifyOfPropertyChange(() => CurrentUser);
                Properties.Settings.Default.CurrentUser = CurrentUser.FirstName;
                // User scope settings gebruikt omdat we anders niet kunnen saven !
                Properties.Settings.Default.Save();
                _eventAggregator.PublishOnUIThread(message: new InitialUserMessage(CurrentUser));
            }
        }

        public double MaxHeight
        {
            get { return _maxHeight; }
            set
            {
                _maxHeight = value;
                NotifyOfPropertyChange(() => MaxHeight);
            }
        }


        public System.Windows.WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }

        private string _tooltipText;

        public string TooltipText
        {
            get { return _tooltipText; }
            set
            {
                _tooltipText = value;
                NotifyOfPropertyChange(() => TooltipText);
            }
        }



        public string ButtonImage
        {
            get { return _buttonImage; }
            set
            {
                _buttonImage = value;
                NotifyOfPropertyChange(() => ButtonImage);
            }
        }

        private bool _leveranciersorBestelbonsVisible;

        public bool LeveranciersorBestelbonsVisible
        {
            get { return _leveranciersorBestelbonsVisible; }
            set
            {
                _leveranciersorBestelbonsVisible = value;
                NotifyOfPropertyChange(() => LeveranciersorBestelbonsVisible);
            }
        }

        private bool _allOK;

        public bool AllOk
        {
            get { return _allOK; }
            set
            {
                _allOK = value;
                NotifyOfPropertyChange(() => AllOk);
            }
        }

        private string _projectDirectory;

        public string ProjectDirectory
        {
            get { return _projectDirectory; }
            set
            {
                _projectDirectory = value;
                NotifyOfPropertyChange(() => ProjectDirectory);
                _eventAggregator.PublishOnUIThread(message: new ProjectDirectoryChange(ProjectDirectory));
            }
        }


        #endregion



        public LeveranciersViewModel LeveranciersVM { get; set; }
        public SelectUserViewModel SelectUserVM { get; set; }
        public BestelbonsViewModel BestelbonsVM { get; set; }
        public BestelbonOpmaakViewModel BestelbonOpmaakVM { get; set; }
        public UsersViewModel UsersVM { get; set; }
        public LeveringsvoorwaardenViewModel LeveringsvoorwaardenVM { get; set; }
        public BindableCollection<User> UserList { get; set; }


        public ShellViewModel(IEventAggregator eventAggregator, IWindowManager windowmanager)
        {
            _windowManager = windowmanager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            ButtonImage = "pack://application:,,,/Resources/MAXIMIZE.png";
            TooltipText = "Maximize";
            this.ErrorList = new BindableCollection<Error>();
            BestelbonsVM = IoC.Get<BestelbonsViewModel>();
            LeveranciersVM = IoC.Get<LeveranciersViewModel>();
            SelectUserVM = IoC.Get<SelectUserViewModel>();
            BestelbonOpmaakVM = IoC.Get<BestelbonOpmaakViewModel>();
            UsersVM = IoC.Get<UsersViewModel>();
            LeveringsvoorwaardenVM = IoC.Get<LeveringsvoorwaardenViewModel>();
            this.UserList = new BindableCollection<User>();
            MaxHeight = screenHeight - 32;

            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //ErrorList.Add("Leverancier Imes ontbreekt adres");
            //ErrorList.Add("Bestelbon hefft nog geen Naam");



            //UserList.Add(new User { Name = "Johan", Email = "johan@astratec.be", Tel = "057/722446" });
            //UserList.Add(new User { Name = "Sarah", Email = "sarah@astratec.be", Tel = "057/722446" });
            //UserList.Add(new User { Name = "Joost", Email = "joost@astratec.be", Tel = "057/722446" });

            //var FilePath = Properties.Settings.Default.UserListPath;
            //var serializer = new XmlSerializer(typeof(BindableCollection<User>));
            //using (var writer = new System.IO.StreamWriter(FilePath))
            //{
            //    serializer.Serialize(writer, UserList);
            //    writer.Flush();
            //}

            SetCurrentUser();

            CheckOK();
        }



        public void LoadLeveranciers()
        {
            _eventAggregator.PublishOnUIThread(message: new LoadLeveranciersMessage());
            LeveranciersorBestelbonsVisible = true;
            ActivateItem(LeveranciersVM);

        }

        public void LoadBestelbons()
        {
            _eventAggregator.PublishOnUIThread(message: new LoadLeveranciersMessage());
            LeveranciersorBestelbonsVisible = true;
            ActivateItem(BestelbonsVM);
        }

        public void LoadUsers()
        {
            //_eventAggregator.PublishOnUIThread(message: new UserListMessage(UserList));
            LeveranciersorBestelbonsVisible = true;
            ActivateItem(UsersVM);
        }

        public void LoadLeveringsvoorwaarden()
        {
            LeveranciersorBestelbonsVisible = true;
            ActivateItem(LeveringsvoorwaardenVM);
            _eventAggregator.PublishOnUIThread(message: new LoadLeveringsvoorwaardenMessage());
        }

        #region WINDOW BUTTONS

        public void MinimizeButton()
        {
            WindowState = WindowState.Minimized;
            var i = screenWidth;
            var j = screenHeight;
        }


        public void MaximizeButton()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    ButtonImage = "pack://application:,,,/Resources/RESTORE.png";
                    TooltipText = "Restore Down";
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    ButtonImage = "pack://application:,,,/Resources/MAXIMIZE.png";
                    TooltipText = "Maximize";
                    break;
                default:
                    break;
            }
        }

        public void CloseButton()
        {
            TryClose();
        }

        #endregion


        #region MENUCOMMANDS

        public void SetLeveranciersSource()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Astratec Leveranciers(*.lev)|*.lev|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"D:\Desktop\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.LeveranciersListPath = openFileDialog.FileName;
                // User scope settings gebruikt omdat we anders niet kunnen saven !
                Properties.Settings.Default.Save();
            }
            CheckOK();
        }

        public void SetBestelbonDirectory()
        {
            string path = "";
            FolderBrowserDialog folderBrowserDialogDialog = new FolderBrowserDialog();
            if (folderBrowserDialogDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialogDialog.SelectedPath))
            {
                path = folderBrowserDialogDialog.SelectedPath;
                Properties.Settings.Default.BestelbonsPath = path;
                // User scope settings gebruikt omdat we anders niet kunnen saven !
                Properties.Settings.Default.Save();
                _eventAggregator.PublishOnUIThread(new BestelbonsPathChanged());
            }

            CheckOK();
        }

        public void SetUsersSource()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Astratec Leveranciers(*.users)|*.users|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"D:\Desktop\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.UserListPath = openFileDialog.FileName;
                // User scope settings gebruikt omdat we anders niet kunnen saven !
                Properties.Settings.Default.Save();

            }

            CheckOK();
        }

        public void SetLeveringsvoorwaarden()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Leveringsvoorwaarden(*.levvw)|*.levvw|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"D:\Desktop\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Leveringsvw = openFileDialog.FileName;
                // User scope settings gebruikt omdat we anders niet kunnen saven !
                Properties.Settings.Default.Save();

            }
            CheckOK();
        }


        public void SelectUser()
        {
            SetCurrentUser();
            SelectUserVM.Capiton = "Select User";
            var result = _windowManager.ShowDialog(SelectUserVM);
        }


        public void SelectProjectDirectory()
        {
            FolderBrowserDialog folderBrowserDialogDialog = new FolderBrowserDialog();
            if (folderBrowserDialogDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialogDialog.SelectedPath))
            {
                ProjectDirectory = folderBrowserDialogDialog.SelectedPath;
            }

        }

        public void Help()
        {
            Process.Start("Bestelbons.chm");
        }

        #endregion

        public void SetCurrentUser()
        {
            string FilePath = Properties.Settings.Default.UserListPath;
            if (!String.IsNullOrEmpty(Properties.Settings.Default.UserListPath))
            {
                var serializer = new XmlSerializer(typeof(BindableCollection<User>));
                using (var stream = File.OpenRead(FilePath))
                {
                    var other = (BindableCollection<User>)(serializer.Deserialize(stream));
                    UserList.Clear();
                    UserList.AddRange(other);
                }

                foreach (var user in UserList)
                {
                    if (Properties.Settings.Default.CurrentUser == user.FirstName)
                    {
                        CurrentUser = user;
                        break;

                    }
                }
                _eventAggregator.PublishOnUIThread(message: new UserListMessage(UserList));
            }
        }

        public void CheckOK()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.BestelbonsPath) &&
                !String.IsNullOrEmpty(Properties.Settings.Default.UserListPath) &&
                !String.IsNullOrEmpty(Properties.Settings.Default.Leveringsvw) &&
                !String.IsNullOrEmpty(Properties.Settings.Default.LeveranciersListPath) &&
                File.Exists(Properties.Settings.Default.LeveranciersListPath) &&
                File.Exists(Properties.Settings.Default.Leveringsvw) &&
                Directory.Exists(Properties.Settings.Default.BestelbonsPath) &&
                File.Exists(Properties.Settings.Default.UserListPath) &&
                !String.IsNullOrEmpty(Properties.Settings.Default.CurrentUser)) AllOk = true;

            if (!String.IsNullOrEmpty(Properties.Settings.Default.BestelbonsPath) &&
                Directory.Exists(Properties.Settings.Default.BestelbonsPath)) UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " Bestelbons Path not set", Active = false });
            else UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " Bestelbons Path not set", Active = true });

            if (!String.IsNullOrEmpty(Properties.Settings.Default.UserListPath) && File.Exists(Properties.Settings.Default.UserListPath)) UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " Userlist Path not set", Active = false });
            else UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " Userlist Path not set", Active = true });

            if (!String.IsNullOrEmpty(Properties.Settings.Default.Leveringsvw) && File.Exists(Properties.Settings.Default.Leveringsvw)) UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " Leveringsvoorwaarden Path not set", Active = false });
            else UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " Leveringsvoorwaarden Path not set", Active = true });

            if (!String.IsNullOrEmpty(Properties.Settings.Default.LeveranciersListPath) && File.Exists(Properties.Settings.Default.LeveranciersListPath)) UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " LeveranciersList Path not set", Active = false });
            else UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " LeveranciersList Path not set", Active = true });

            if (!String.IsNullOrEmpty(Properties.Settings.Default.CurrentUser)) UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " User not set", Active = false });
            else UpdateErrorList(new Error { Level = ErrorLevel.Error, ErrorMessage = " User not set", Active = true });
        }


        public void UpdateErrorList(Error error)
        {
            if (!error.Active)
            {
                foreach (var err in ErrorList)
                {
                    if (err.ErrorMessage == error.ErrorMessage)
                    {
                        ErrorList.Remove(err);
                        break;
                    }

                }
            }
            else
            {
                bool AlreadyInList = false;
                foreach (var err in ErrorList)
                {
                    if (err.ErrorMessage == error.ErrorMessage)
                    {
                        AlreadyInList = true;
                    }

                }
                if (!AlreadyInList)
                    ErrorList.Add(error);
            }

        }

        public void Handle(LeveranciersChange message)
        {
            //LeveranciersorBestelbonsVisible = false;
        }

        public void Handle(UserChangedMessage message)
        {
            if (message.user != null)
            {
                CurrentUser = message.user;
            }
            CheckOK();
        }

        public void Handle(CloseMessage message)
        {
            LeveranciersorBestelbonsVisible = false;
        }

        public void Handle(ErrorMessage error)
        {
            UpdateErrorList(error.Error);
        }
    }
}
