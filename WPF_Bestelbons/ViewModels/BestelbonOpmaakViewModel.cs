using Caliburn.Micro;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using WPF_Bestelbons.Models;
using static WPF_Bestelbons.Models.Bestelbonregel;
using Exception = System.Exception;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace WPF_Bestelbons.ViewModels
{
    public class BestelbonOpmaakViewModel : Caliburn.Micro.Screen, IHandle<LeveranciersChange>,
        IHandle<BestelbonChange>, IHandle<UserChangedMessage>, IHandle<InitialUserMessage>,
        IHandle<ProjectDirectoryChange>, IHandle<LeveranciersListUpdated>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private PDFCreator _pdfCreator;
        private Bestelbon _bestelbon;
        private Leverancier _leverancier;

        public BindableCollection<Leverancier> LeveranciersList { get; set; }
        public User CurrentUser { get; set; }
        public string ProjectDirectory { get; set; }

        public string FilePath;

        public bool CanDeleteBestelregel
        {
            get { return BestelbonregelsSelectedItem != null; }
        }

        public bool CanAdd
        {
            get { return !String.IsNullOrEmpty(NewBestelregel); }
        }

        public bool CanSave
        {
            get
            {
                return (!String.IsNullOrEmpty(LeveranciersNaamUI) && !String.IsNullOrEmpty(ProjectNumber) &&
                        !String.IsNullOrEmpty(VolgNummer));
            }
        }

        public bool CanSaveMail
        {
            get
            {
                return (!String.IsNullOrEmpty(Leverancier.Email) && CanSave);
            }

        }

        public bool CanSaveAttach
        {
            get
            {
                return CanSave;

            }
        }

        private BindableCollection<Bestelbonregel> _bestelbonregels;

        public BindableCollection<Bestelbonregel> Bestelbonregels
        {
            get { return _bestelbonregels; }
            set
            {
                _bestelbonregels = value;
                NotifyOfPropertyChange(() => Bestelbonregels);
            }
        }

        private Bestelbonregel _bestelbonregelSelectedItem;

        public Bestelbonregel BestelbonregelsSelectedItem
        {
            get { return _bestelbonregelSelectedItem; }
            set
            {
                _bestelbonregelSelectedItem = value;
                NotifyOfPropertyChange(() => CanDeleteBestelregel);
                NotifyOfPropertyChange(() => BestelbonregelsSelectedItem);
            }
        }

        private bool _projDirOK;

        public bool ProjDirOK
        {
            get { return _projDirOK; }
            set
            {
                _projDirOK = value;
                NotifyOfPropertyChange(() => ProjDirOK);
            }
        }

        private string _leveranciersNaamUI;

        public string LeveranciersNaamUI
        {
            get { return _leveranciersNaamUI; }
            set
            {
                _leveranciersNaamUI = value;
                NotifyOfPropertyChange(() => LeveranciersNaamUI);
                SetBestelbonNaam();
            }
        }

        private string _projectNumber;

        public string ProjectNumber
        {
            get { return _projectNumber; }
            set
            {
                _projectNumber = value;
                NotifyOfPropertyChange(() => ProjectNumber);
                SetBestelbonNaam();
            }
        }

        private string _volgNummer;

        public string VolgNummer
        {
            get { return _volgNummer; }
            set
            {
                _volgNummer = value;
                NotifyOfPropertyChange(() => VolgNummer);
                SetBestelbonNaam();
            }
        }

        private string _opmerking;

        public string Opmerking
        {
            get { return _opmerking; }
            set
            {
                _opmerking = value;
                NotifyOfPropertyChange(() => Opmerking);
            }
        }

        #region FULLPROPERTIES NEW BESTELREGEL

        private int _newAantal;

        public int NewAantal
        {
            get { return _newAantal; }
            set
            {
                _newAantal = value;
                BerekenTotalePrijsNewBestelregel();
                NotifyOfPropertyChange(() => NewAantal);
            }
        }

        private string _newEenheid;

        public string NewEenheid
        {
            get { return _newEenheid; }
            set
            {
                _newEenheid = value;
                NotifyOfPropertyChange(() => NewEenheid);
            }
        }

        private string _newBestelregel;

        public string NewBestelregel
        {
            get { return _newBestelregel; }
            set
            {
                _newBestelregel = value;
                NotifyOfPropertyChange(() => CanAdd);
                NotifyOfPropertyChange(() => NewBestelregel);
            }
        }


        private string _newPrijsstring;

        public string NewPrijsstring
        {
            get { return _newPrijsstring; }
            set
            {
                _newPrijsstring = value;
                NotifyOfPropertyChange(() => NewPrijsstring);

                try
                {
                    NewPrijs = Decimal.Parse(NewPrijsstring.Replace(".", ","), NumberStyles.AllowDecimalPoint);
                }
                catch (Exception)
                {

                    NewPrijs = 0;
                }
            }
        }

        private decimal _newPrijs;

        public decimal NewPrijs
        {
            get { return _newPrijs; }
            set
            {
                _newPrijs = value;
                BerekenTotalePrijsNewBestelregel();
                NotifyOfPropertyChange(() => NewPrijs);
            }
        }

        private decimal _newTotalePrijs;

        public decimal NewTotalePrijs
        {
            get { return _newTotalePrijs; }
            set
            {
                _newTotalePrijs = value;
                NotifyOfPropertyChange(() => NewTotalePrijs);
            }
        }

        #endregion

        private decimal _totaal;

        public decimal Totaal
        {
            get { return _totaal; }
            set
            {
                _totaal = value;
                NotifyOfPropertyChange(() => Totaal);
            }
        }

        private string _bestelbonNaam;

        public string BestelbonNaam
        {
            get { return _bestelbonNaam; }
            set
            {
                _bestelbonNaam = value;
                NotifyOfPropertyChange(() => BestelbonNaam);
                NotifyOfPropertyChange(() => CanSave);
                NotifyOfPropertyChange(() => CanSaveMail);
                NotifyOfPropertyChange(() => CanSaveAttach);
                Bestelbon.Name = BestelbonNaam;
                if (!String.IsNullOrEmpty(LeveranciersNaamUI) && !String.IsNullOrEmpty(ProjectNumber) && !String.IsNullOrEmpty(VolgNummer))
                    _eventAggregator.PublishOnUIThread(new ErrorMessage(new Error
                    { Level = ErrorLevel.Error, ErrorMessage = " Bestelbon Naam not set", Active = false }));
                else
                    _eventAggregator.PublishOnUIThread(new ErrorMessage(new Error
                    { Level = ErrorLevel.Error, ErrorMessage = " Bestelbon Naam not set", Active = true }));
            }
        }


        public Bestelbon Bestelbon
        {
            get { return _bestelbon; }
            set
            {
                _bestelbon = value;
                NotifyOfPropertyChange(() => Bestelbon);
            }
        }

        public PDFCreator PdfCreator
        {
            get { return _pdfCreator; }
            set { _pdfCreator = value; }
        }


        public Leverancier Leverancier
        {
            get { return _leverancier; }
            set
            {
                _leverancier = value;
                NotifyOfPropertyChange(() => Leverancier);
                NotifyOfPropertyChange(() => CanSaveMail);
                if (Leverancier.Name == null)
                    _eventAggregator.PublishOnUIThread(new ErrorMessage(new Error
                    { Level = ErrorLevel.Error, ErrorMessage = " Leverancier not set", Active = true }));
                else
                    _eventAggregator.PublishOnUIThread(new ErrorMessage(new Error
                    { Level = ErrorLevel.Error, ErrorMessage = " Leverancier not set", Active = false }));
                if (String.IsNullOrEmpty(Leverancier.Email))
                    _eventAggregator.PublishOnUIThread(new ErrorMessage(new Error
                    { Level = ErrorLevel.Warning, ErrorMessage = " Email Leverancier not set", Active = true }));
                else
                    _eventAggregator.PublishOnUIThread(new ErrorMessage(new Error
                    { Level = ErrorLevel.Warning, ErrorMessage = " Email Leverancier not set", Active = false }));
            }
        }


        public BestelbonOpmaakViewModel(IEventAggregator eventAggregator, IWindowManager windowsmanager,
            Bestelbon bestelbon, PDFCreator pDFCreator)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowsmanager;
            _eventAggregator.Subscribe(this);
            _bestelbon = bestelbon;
            _pdfCreator = pDFCreator;
            this.Leverancier = new Leverancier();
            LeveranciersNaamUI = "";
            Opmerking = "Te vermelden bij communicatie : ";
            NewEenheid = "stuk";
            Bestelbon.Leverancier = new Leverancier();
            Bestelbonregels = Bestelbon.Bestelbonregels;
            Totaal = Bestelbon.TotalPrice;
            Bestelbon.OnTotalPriceChanged += BerekenPrijs;
            NewAantal = 1;
            NewPrijsstring = "0.0";
            BestelbonNaam = "";
            this.CurrentUser = new User();

            //String levvw = "1. Maximum hefvermogen beschikbare heftruck = 2.5 ton hefhoogte = 3m";
            //var FilePath = Properties.Settings.Default.Leveringsvw;
            //var serializer = new XmlSerializer(typeof(String));
            //using (var writer = new System.IO.StreamWriter(FilePath))
            //{
            //    serializer.Serialize(writer, levvw);
            //    writer.Flush();
            //}

        }



        #region COMMANDS

        public void Add()
        {
            EA Eenheid = (EA)Enum.Parse(typeof(EA), NewEenheid);
            Bestelbon.Bestelbonregels.Add(new Bestelbonregel()
            {
                Aantal = NewAantal,
                Eenheid = NewEenheid,
                Bestelregel = NewBestelregel,
                Prijsstring = NewPrijsstring,
                Prijs = NewPrijs,
                TotalePrijs = NewTotalePrijs
            });
            Bestelbon.CalculateTotalPrice();
            Bestelbonregels = Bestelbon.Bestelbonregels;
        }


        public void Save()
        {
            Bestelbon.Name = BestelbonNaam;
            Bestelbon.Leverancier.Name = Leverancier.Name;
            Bestelbon.Bestelbonregels = Bestelbonregels;
            Bestelbon.Opmerking = Opmerking;
            Bestelbon.ProjectDirectory = ProjectDirectory;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Astratec Bestelbons(*.abb)|*.abb|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save Bestelbon";
            saveFileDialog1.FileName = Bestelbon.Name;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = saveFileDialog1.FileName;

                string FilePathLevvw = Properties.Settings.Default.Leveringsvw;
                var serializerlevvw = new XmlSerializer(typeof(String));
                using (var stream = File.OpenRead(FilePathLevvw))
                {
                    var other = (String)(serializerlevvw.Deserialize(stream));
                    Bestelbon.Leveringsvoorwaarden = "";
                    Bestelbon.Leveringsvoorwaarden = other;
                }


                using (var writer = new System.IO.StreamWriter(FilePath))
                {
                    var serializer = new XmlSerializer(typeof(Bestelbon));
                    serializer.Serialize(writer, Bestelbon);
                    writer.Flush();
                }



                if (Directory.Exists(ProjectDirectory))
                {
                    try
                    {
                        using (var writer = new System.IO.StreamWriter(ProjectDirectory + "\\" + BestelbonNaam))
                        {
                            var serializer = new XmlSerializer(typeof(Bestelbon));
                            serializer.Serialize(writer, Bestelbon);
                            writer.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                        var dialogViewModel = IoC.Get<DialogViewModel>();
                        dialogViewModel.Capiton = "File Open";
                        dialogViewModel.Message = e.ToString();
                        var result = _windowManager.ShowDialog(dialogViewModel);

                    }

                }

                try
                {
                    FileStream fs = new FileStream(FilePath + ".pdf", FileMode.Create);
                    PdfCreator.Create(Bestelbon, CurrentUser, fs);
                    fs.Close();
                    FileStream fsProjectDirectory =
                        new FileStream(ProjectDirectory + "\\" + BestelbonNaam + ".pdf", FileMode.Create);
                    PdfCreator.Create(Bestelbon, CurrentUser, fsProjectDirectory);
                    fsProjectDirectory.Close();
                }
                catch (Exception ex)
                {
                    var dialog = IoC.Get<DialogViewModel>();
                    dialog.Capiton = " Creating file and PDF";
                    dialog.Message = ex.ToString();
                    _windowManager.ShowDialog(dialog);

                }
            }

            _eventAggregator.PublishOnUIThread(message: new BestelbonAdded());

        }

        public void SaveMail()
        {
            Save();

            string Attachmentfilename = $"{Properties.Settings.Default.BestelbonsPath}\\{BestelbonNaam}.pdf";

            Outlook.Application OutlookApplication = new Outlook.Application();
            Outlook.Folder calFolder =
                OutlookApplication.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar) as
                    Outlook.Folder;
            Outlook.MailItem mailItem =
                (Outlook.MailItem)OutlookApplication.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = $"Bestelbon Project {ProjectNumber}";
            mailItem.To = Leverancier.Email;
            try
            {
                mailItem.Attachments.Add(Attachmentfilename, Outlook.OlAttachmentType.olByValue, 1, Attachmentfilename);
                mailItem.Send();
            }
            catch (Exception e)
            {
                var dialogViewModel = IoC.Get<DialogViewModel>();
                dialogViewModel.Capiton = "Attachment Error";
                dialogViewModel.Message = e.ToString();
                var result = _windowManager.ShowDialog(dialogViewModel);
            }


        }

        public void SaveAttach()
        {
            string AttachedPDF;
            Bestelbon.Name = BestelbonNaam;
            Bestelbon.Leverancier.Name = Leverancier.Name;
            Bestelbon.Bestelbonregels = Bestelbonregels;
            Bestelbon.Opmerking = Opmerking;
            Bestelbon.ProjectDirectory = ProjectDirectory;



            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Astratec Bestelbons(*.abb)|*.abb|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save Bestelbon";
            saveFileDialog1.FileName = Bestelbon.Name;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = saveFileDialog1.FileName;

                string FilePathLevvw = Properties.Settings.Default.Leveringsvw;
                var serializerlevvw = new XmlSerializer(typeof(String));
                using (var stream = File.OpenRead(FilePathLevvw))
                {
                    var other = (String)(serializerlevvw.Deserialize(stream));
                    Bestelbon.Leveringsvoorwaarden = "";
                    Bestelbon.Leveringsvoorwaarden = other;
                }


                using (var writer = new System.IO.StreamWriter(FilePath))
                {
                    var serializer = new XmlSerializer(typeof(Bestelbon));
                    serializer.Serialize(writer, Bestelbon);
                    writer.Flush();
                }



                if (Directory.Exists(ProjectDirectory))
                {
                    try
                    {
                        using (var writer = new System.IO.StreamWriter(ProjectDirectory + "\\" + BestelbonNaam))
                        {
                            var serializer = new XmlSerializer(typeof(Bestelbon));
                            serializer.Serialize(writer, Bestelbon);
                            writer.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                        var dialogViewModel = IoC.Get<DialogViewModel>();
                        dialogViewModel.Capiton = "File Open";
                        dialogViewModel.Message = e.ToString();
                        var result = _windowManager.ShowDialog(dialogViewModel);

                    }

                }

                OpenFileDialog openfiledialog = new OpenFileDialog();
                openfiledialog.Title = "Attach PFD file";
                openfiledialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                if (openfiledialog.ShowDialog() == DialogResult.OK)
                {
                    string CopiedPDFfilename = $"{Properties.Settings.Default.BestelbonsPath}\\{BestelbonNaam}.pdf";
                    AttachedPDF = openfiledialog.FileName;
                    File.Copy(AttachedPDF, CopiedPDFfilename);
                }


            }

            _eventAggregator.PublishOnUIThread(message: new BestelbonAdded());
        }

        public void NewBestelbon()
        {
            Bestelbon.Bestelbonregels.Clear();
            ProjectNumber = "";
            VolgNummer = "";
            LeveranciersNaamUI = "";
            Bestelbon.Opmerking = "";
            Bestelbon.TotalPrice = 0.0M;
            BestelbonNaam = "";
            Opmerking = "Te vermelden bij communicatie : ";
            NewBestelbonregel();

        }

        public void NewBestelbonregel()
        {
            NewAantal = 1;
            NewBestelregel = "";
            NewPrijsstring = "0.0";
            NewTotalePrijs = 0.0M;
        }


        public void DeleteBestelregel()
        {
            Bestelbon.Bestelbonregels.Remove(BestelbonregelsSelectedItem);
            Bestelbon.CalculateTotalPrice();

        }

        #endregion

        public void BerekenPrijs()
        {
            Totaal = Bestelbon.TotalPrice;
        }

        public void BerekenTotalePrijsNewBestelregel()
        {
            NewTotalePrijs = NewAantal * NewPrijs;

        }

        public void BestelregelChange()
        {
            Bestelbon.CalculateTotalPrice();
        }

        public void SetBestelbonNaam()
        {
            BestelbonNaam = $"{ProjectNumber}-{LeveranciersNaamUI}-{VolgNummer}";
        }

        public void SetProjDirOK()
        {
            ProjDirOK = !String.IsNullOrEmpty(ProjectDirectory);
        }

        public void Handle(BestelbonChange filename)
        {
            NewBestelbonregel();
            NewBestelbon();

            string path = Properties.Settings.Default.BestelbonsPath + "\\" + filename.Bestelbon;
            if (!string.IsNullOrEmpty(path))
            {

                using (var stream = System.IO.File.OpenRead(path))
                {
                    var serializer = new XmlSerializer(typeof(Bestelbon));
                    Bestelbon = serializer.Deserialize(stream) as Bestelbon;
                }

                ProjectDirectory = Bestelbon.ProjectDirectory;
                SetProjDirOK();
                Bestelbon.OnTotalPriceChanged += BerekenPrijs;
                Bestelbon.CalculateTotalPrice();
                Bestelbonregels = Bestelbon.Bestelbonregels;
                string[] data = Bestelbon.Name.Split('-');

                try
                {
                    ProjectNumber = data[0];
                    VolgNummer = data[2];
                }
                catch (Exception)
                {

                }

                foreach (var lev in LeveranciersList)
                {
                    if (lev.Name == Bestelbon.Leverancier.Name)
                    {
                        Leverancier = lev;
                        LeveranciersNaamUI = Leverancier.Name;
                        break;
                    }
                }

                Opmerking = Bestelbon.Opmerking;

                //BestelbonNaam = Bestelbon.Name;
                Totaal = Bestelbon.TotalPrice;

            }

        }

        public void Handle(UserChangedMessage message)
        {
            CurrentUser = message.user;
        }

        public void Handle(InitialUserMessage message)
        {
            CurrentUser = message.user;
        }

        public void Handle(ProjectDirectoryChange projectDirectory)
        {
            ProjectDirectory = projectDirectory.ProjectDirectory;
            SetProjDirOK();
        }

        public void Handle(LeveranciersListUpdated leverancierslist)
        {
            LeveranciersList = leverancierslist.Updatedleverancierslist;
        }

        public void Handle(LeveranciersChange message)
        {
            Leverancier = message.Leverancier;
            LeveranciersNaamUI = message.Leverancier.Name;


        }
    }
}

