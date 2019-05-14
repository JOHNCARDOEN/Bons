using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using WPF_Bestelbons.Models;

namespace WPF_Bestelbons
{
   public class LeveranciersListUpdated
    {
        public BindableCollection<Leverancier> Updatedleverancierslist { get; set; }

        public LeveranciersListUpdated(BindableCollection<Leverancier> LeveranciersList)
        {
            Updatedleverancierslist = LeveranciersList;
        }
    }
}
