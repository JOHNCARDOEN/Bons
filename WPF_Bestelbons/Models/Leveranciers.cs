using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bestelbons.Models
{
     public class Leveranciers
    {
        public BindableCollection<Leverancier> LeveranciersList;

        public Leveranciers()
        {
            LeveranciersList = new BindableCollection<Leverancier>();
        }

    }
}
