using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bestelbons.Models
{
    public class Bestelbonregel : PropertyChangedBase

    {

        private int _aantal;

        public int Aantal
        {
            get { return _aantal; }
            set
            {
                _aantal = value;
                NotifyOfPropertyChange(() => Aantal);
                CalculateTotalPrice();
            }
        }

        private string eA;

        public string Eenheid
        {
            get { return eA; }
            set { eA = value; }
        }


        private string _bestelregel;

        public string Bestelregel
        {
            get { return _bestelregel; }
            set { _bestelregel = value; }

        }

        private string _prijsstring;

        public string Prijsstring
        {
            get { return _prijsstring; }
            set
            {
                _prijsstring = value; 
                NotifyOfPropertyChange(() => Prijsstring);

                try
                {
                    Prijs = Decimal.Parse(Prijsstring.Replace(".", ","), NumberStyles.AllowDecimalPoint);
                }
                catch (Exception)
                {

                    Prijs = 0;
                }
            }
        }

        private decimal _prijs;

        public decimal Prijs
        {
            get { return _prijs; }
            set
            {
                _prijs = value;
                NotifyOfPropertyChange(() => Prijs);
                CalculateTotalPrice();
            }
        }

        private decimal _totalePrijs;

        public decimal TotalePrijs
        {
            get { return _totalePrijs; }
            set { _totalePrijs = value;
                NotifyOfPropertyChange(() => TotalePrijs);
            }
        }


        public enum EA
        {
            stuk,
            doos,
        }

        public void CalculateTotalPrice()
        {
            TotalePrijs = Prijs * Aantal;
        }

    }
}
