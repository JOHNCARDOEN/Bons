using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bestelbons.Models
{
    public class Bestelbon : PropertyChangedBase
    {
        public delegate void dgEventRaiser();
        public event dgEventRaiser OnTotalPriceChanged;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _opmerking;

        public string Opmerking
        {
            get { return _opmerking; }
            set { _opmerking = value; }
        }

        private string _leveringsvoorwaarden;
        public string Leveringsvoorwaarden
        {
            get { return _leveringsvoorwaarden; }
            set { _leveringsvoorwaarden = value; }
        }

        private Leverancier leverancier;

        public Leverancier Leverancier
        {
            get { return leverancier; }
            set { leverancier = value; }
        }

        private string _projectDirectory;

        public string ProjectDirectory
        {
            get { return _projectDirectory; }
            set
            {
                _projectDirectory = value;
            }
        }   

        private decimal _totalPrice;

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                NotifyOfPropertyChange(() => TotalPrice);
                OnTotalPriceChanged?.Invoke();
            }
        }


        private BindableCollection<Bestelbonregel> _bestelbonregels;

        public BindableCollection<Bestelbonregel> Bestelbonregels
        {
            get { return _bestelbonregels; }
            set { _bestelbonregels = value; }
        }

        public Bestelbon()
        {
            Name = "Bestelbon  ";
            Bestelbonregels = new BindableCollection<Bestelbonregel>();
        }

        public void CalculateTotalPrice()
        {
            TotalPrice = 0.0M;
            for (int i = 0; i < Bestelbonregels.Count; i++)
            {
                TotalPrice += Bestelbonregels[i].TotalePrijs;
            }
        }

        //public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //        foreach (Bestelbonregel item in e.NewItems)
        //            item.PropertyChanged += Bestelbonregel_PropertyChanged;

        //    if (e.OldItems != null)
        //        foreach (Bestelbonregel item in e.OldItems)
        //            item.PropertyChanged -= Bestelbonregel_PropertyChanged;
        //}
        //public void Bestelbonregel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "TotalePrijs") CalculateTotalPrice() ;
        //}
    }
}
